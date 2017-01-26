using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using static PetriNetCsharp.Helpers;

namespace PetriNetCsharp
{
    class PetriNet
    {
        /// <summary>
        /// sss
        /// </summary>

        //private //members
        private List<List<int>> _Tcond; //macierz warunki konieczne odpalenia dla kazdej tranzycji [TxP]
        private List<bool> _TReady; //macierz tranzycji gotowych do odpalenia[Tx1]
        private List<List<int>> _Teye;    //macierz diagonalna wektorow jedynkowych do wyznaczenia warunkow odpalania tranzycji [TxT]
        //private List<List<int>> _D;     //macierz incydencji transponowana	[TxP]
        private int _currentlyCheckedTransition; //tranzycja aktualnie testowana
        //private List<List<int>> _tmp2D;
        
        //public members
        [JsonIgnore]
        public int NumberOfPlaces { get;  set; }
        [JsonIgnore]
        public int NumberOfTransitions { get;  set; }
        [JsonIgnore]
        public List<List<int>> Dmatrix { get; set; }     //macierz incydencji transponowana	[TxP]
        [JsonProperty]
        public List<List<int>> DinMatrix { get;  set; }   //macierz wejsc - dodawania zetonow [TxP]
        [JsonProperty]
        public List<List<int>> DoutMatrix { get;   set; }  //macierz wyjsc - odejmowania zetonow [TxP]
        [JsonIgnore]
        public List<List<int>> Tcond { get;  set; }   //macierz warunki konieczne odpalenia dla kazdej tranzycji [TxP]????
        [JsonIgnore]
        public List<bool> TReady { get;  set; }         //wektro tranzycji gotowych do odpalenia po sprawdzeniu warunkow [T]
        [JsonIgnore]
        public int CurrentlyCheckedTransition { get;  set; } //tranzycja aktualnie testowana
        [JsonProperty]
        public List<int> Mbegin { get;  set; }        //wektro znakowania poczatkowego [P]
        [JsonIgnore]
        public List<int> Mcurrent { get;  set; }        //wektro znakowania poczatkowego [P]
        [JsonIgnore]
        public int CurrentStep { get;  set; }

        //konstruktory
        public PetriNet() { }
        public PetriNet(DataGridView dgvDin, DataGridView dgvDout, DataGridView dgvMo)   //konstruktor
        {
            CurrentStep = 1;
            _currentlyCheckedTransition = 0;
            DinMatrix = ImportDataGridToMatrix2D(dgvDin);
            DoutMatrix = ImportDataGridToMatrix2D(dgvDout);
            Mbegin = ImportDataGridToMatrix2D(dgvMo)[0];
            Mcurrent = Mbegin;

            NumberOfTransitions = DinMatrix.Count;
            NumberOfPlaces = DinMatrix[0].Count;

            //generowanie macierzy incydencji ==> Dmatrix = DinMatrix - DoutMatrix
            Dmatrix = MakeIncidenceMatrix();

            //generowanie macierzy Tcond - warunkow odpalenia tranzycji
            //warunki odpalenia dla kazdej trnazycji
            //		Tcond = Teye * DoutMatrix;
            Tcond = MakeTransitionConditions();

            TReady = MakeTreadyVector();//to jest tylko do pokazania w dgv bo i tak trza to sprawdzac po kolei pojedynczo

        }

        public PetriNet(List<List<int>> ListDin, List<List<int>> ListDout, List<int> ListMo)   //konstruktor
        {
            CurrentStep = 1;
            _currentlyCheckedTransition = 0;
            DinMatrix = ListDin;
            DoutMatrix = ListDout;
            Mbegin = ListMo;
            Mcurrent = Mbegin;

            NumberOfTransitions = DinMatrix.Count;
            NumberOfPlaces = DinMatrix[0].Count;

            //generowanie macierzy incydencji ==> Dmatrix = DinMatrix - DoutMatrix
            Dmatrix = MakeIncidenceMatrix();

            //generowanie macierzy Tcond - warunkow odpalenia tranzycji
            //warunki odpalenia dla kazdej trnazycji
            //		Tcond = Teye * DoutMatrix;
            Tcond = MakeTransitionConditions();

            TReady = MakeTreadyVector();//to jest tylko do pokazania w dgv bo i tak trza to sprawdzac po kolei pojedynczo

        }


        //private //methods
        private List<bool> MakeTreadyVector()
        {
            try
            {
                _TReady = new List<bool>(NumberOfTransitions);
                
                for (int i = 0; i < _TReady.Capacity; i++)
                {
                    _TReady.Add(IntToBool(IsTransitionReadyToFire(_Tcond[i])));
                }
               return _TReady;
                }
            catch (Exception ex )
            {
                Report(ex);
                return null;
            }
        }

        private List<List<int>> MakeIncidenceMatrix()    //generowanie macierzy incydencji ==> Dmatrix=DinMatrix-DoutMatrix
        {
            return SubtractElementWise(DinMatrix, DoutMatrix);
        }
        private List<List<int>> MakeTransitionConditions()
        {
            //		generowanie wektorow jednostkowych T[P]=eye dl akazdej tranzycji
            _Teye = new List<List<int>>(NumberOfTransitions);
            for (int i = 0; i < NumberOfTransitions; i++)
            {
                _Teye.Add(new List<int>());
                for (int j = 0; j < NumberOfTransitions; j++)
                {
                    _Teye[i].Add(i == j ? 1 : 0);
                }
            }

            //		mnozenie macierzy
            _Tcond = Matrix2DMultiply(_Teye, DoutMatrix);
            return _Tcond;
        }
        private int IsTransitionReadyToFire(List<int> condition)
        {
            //tu jest int zamiast bool bo kopiowalem z Cpp ktore robilem na arduino
            int result = 1;
            int check = 1;
            for (int i = 0; i < NumberOfPlaces; i++)    //czy warunek spelniony dla wszystkich miejsc
            {
                //teraz dla kazdego miejsca sprawdzamy czy caly warunek jest spelniony
                //czy liczba znacznikow w M jest >= od wymaganych w tranzycji
                if (Mcurrent[i] < condition[i])
                {
                    check = 0;
                }
            }
            return result*check;
        }

        //public //methods
        public void NextStep()
        {
            //to trzeba pojedynczko po kolei bo kazda tranzycja moze zmienc nam stan sieci
            //wiec nie mozemy przemnozyc wszytskiego naraz
            if (IsTransitionReadyToFire(_Tcond[_currentlyCheckedTransition]) == 1)
            {
                //teraz jesli warunek spelniony to uaktualniamy znakowanie, 
                //kolejna tranzycja bedzie operowac na tym nowym znakowaniu
                //newM=M+Teye[i]*Dmatrix, czyli nei trzeba nawet mnozyc macierzy

                Mcurrent= AddElementWise(Mcurrent, Dmatrix[_currentlyCheckedTransition]);
            }
            _currentlyCheckedTransition++;
            if (_currentlyCheckedTransition >= NumberOfTransitions)
                _currentlyCheckedTransition = 0;
            TReady = MakeTreadyVector();
            CurrentStep++;
        }
        

        //######HELPERS####

    }
}



