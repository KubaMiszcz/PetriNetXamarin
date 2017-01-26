using System.Collections.Generic;

namespace PetriNetXam
{
    public class PetriNet
    {
        public string CurrentMatrix = "Din";

        public PetriNet()
        {
        }

        public PetriNet(List<List<int>> ListDin, List<List<int>> ListDout, List<int> ListMo) //konstruktor
        {
            CurrentStep = 1;
            CurrentlyCheckedTransition = 0;
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
            //czyli w sumie to: T is ready jesli Mcurrent>=Dout[i]   
            //i to i-ty wiersz Dout i tak po kolei, a potem od poczatku ale za kazdym razem aktualizujemy Mcurrent
        }

        /// ####################  wersja na ANDROIDA  ################

        //private //members
        //private List<List<int>> _Tcond; //macierz warunki konieczne odpalenia dla kazdej tranzycji [TxP]
        //private List<bool> _TReady; //macierz tranzycji gotowych do odpalenia[Tx1]
        //private List<List<int>> _Teye;    //macierz diagonalna wektorow jedynkowych do wyznaczenia warunkow odpalania tranzycji [TxT]
        //private List<List<int>> _D;     //macierz incydencji transponowana	[TxP]
        //private int _currentlyCheckedTransition; //tranzycja aktualnie testowana
        //private List<List<int>> _tmp2D;

        //public members
        public int NumberOfPlaces { get; set; }

        public int NumberOfTransitions { get; set; }
        public List<List<int>> Dmatrix { get; set; } //macierz incydencji transponowana	[TxP]
        public List<List<int>> DinMatrix { get; set; } //macierz wejsc - dodawania zetonow [TxP]
        public List<List<int>> DoutMatrix { get; set; } //macierz wyjsc - odejmowania zetonow [TxP]
        //public List<List<int>> Tcond { get; set; }   //macierz warunki konieczne odpalenia dla kazdej tranzycji [TxP]????
        //public List<bool> TReady { get; set; }         //wektro tranzycji gotowych do odpalenia po sprawdzeniu warunkow [T]
        public int CurrentlyCheckedTransition { get; set; } //tranzycja aktualnie testowana
        public List<int> Mbegin { get; set; } //wektro znakowania poczatkowego [P]
        public List<int> Mcurrent { get; set; } //wektro znakowania poczatkowego [P]
        public int CurrentStep { get; set; }

        private List<List<int>> MakeIncidenceMatrix() //generowanie macierzy incydencji ==> Dmatrix=DinMatrix-DoutMatrix
        {
            return Helpers.SubtractElementWise(DinMatrix, DoutMatrix);
        }

        public void FIllWithExampleData()
        {
            NumberOfPlaces = 4;
            NumberOfTransitions = 3;
            Mbegin = new List<int>(new[] {1, 0, 0, 0});
            Mcurrent = Mbegin;

            DinMatrix = new List<List<int>>();
            DinMatrix.Add(Helpers.CreateList(1, 0, 0, 0));
            DinMatrix.Add(Helpers.CreateList(0, 1, 1, 0));
            DinMatrix.Add(Helpers.CreateList(0, 0, 0, 1));

            DoutMatrix = new List<List<int>>();
            DoutMatrix.Add(Helpers.CreateList(0, 1, 1, 0));
            DoutMatrix.Add(Helpers.CreateList(0, 0, 0, 1));
            DoutMatrix.Add(Helpers.CreateList(1, 0, 0, 0));

            Dmatrix = new List<List<int>>();
            Dmatrix = Helpers.SubtractElementWise(DinMatrix, DoutMatrix);
        }
    }
}