using System.Collections.Generic;
using System.Linq;
using Java.Nio.Channels;

namespace PetriNetXamarin
{
    public class PetriNet
    {
        /// ####################  wersja na ANDROIDA  ################
        //private //members
        //private List<List<int>> _Tcond; //macierz warunki konieczne odpalenia dla kazdej tranzycji [TxP]
        //private List<bool> _TReady; //macierz tranzycji gotowych do odpalenia[Tx1]
        //private List<List<int>> _Teye;    //macierz diagonalna wektorow jedynkowych do wyznaczenia warunkow odpalania tranzycji [TxT]
        //private List<List<int>> _D;     //macierz incydencji transponowana	[TxP]
        //private int _currentlyCheckedTransition; //tranzycja aktualnie testowana
        //private List<List<int>> _tmp2D;

        //public members
        public string CurrentMatrix = "Din";

        public int NumberOfPlaces { get; set; }
        public int NumberOfTransitions { get; set; }
        public List<List<int>> DincidenceMatrix { get; set; } //macierz incydencji transponowana	[TxP]
        public List<List<int>> DinMatrix { get; set; } //macierz wejsc - dodawania zetonow [TxP]
        public List<List<int>> DoutMatrix { get; set; } //macierz wyjsc - odejmowania zetonow [TxP]
        //public List<List<int>> Tcond { get; set; }   //macierz warunki konieczne odpalenia dla kazdej tranzycji [TxP]????
        //public List<bool> TReady { get; set; }         //wektro tranzycji gotowych do odpalenia po sprawdzeniu warunkow [T]
        public int CurrentlyCheckedTransition { get; set; } //tranzycja aktualnie testowana
        public List<int> Mbegin { get; set; } //wektro znakowania poczatkowego [P]
        public List<int> Mcurrent { get; set; } //wektro znakowania poczatkowego [P]
        public int CurrentStep { get; set; }

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

            //generowanie macierzy incydencji ==> DincidenceMatrix = DinMatrix - DoutMatrix
            MakeIncidenceMatrix();
            //generowanie macierzy Tcond - warunkow odpalenia tranzycji
            //warunki odpalenia dla kazdej trnazycji
            //		Tcond = Teye * DoutMatrix;
            //czyli w sumie to: T is ready jesli Mcurrent>=Dout[i]   
            //i to i-ty wiersz Dout i tak po kolei, a potem od poczatku ale za kazdym razem aktualizujemy Mcurrent
        }

        public void MakeIncidenceMatrix() //generowanie macierzy incydencji ==> DincidenceMatrix=DinMatrix-DoutMatrix
        {
            DincidenceMatrix = Helpers.SubtractElementWise(DinMatrix, DoutMatrix);
        }

        public void NextStep()
        {
            //to trzeba pojedynczko po kolei bo kazda tranzycja moze zmienc nam stan sieci
            //wiec nie mozemy przemnozyc wszytskiego naraz
            if (IsTransitionReadyToFire(DoutMatrix[(CurrentlyCheckedTransition - 1)]))
                //minus jeden bo sprawadzam wiersz macierzy
            {
                //teraz jesli warunek spelniony to uaktualniamy znakowanie, 
                //kolejna tranzycja bedzie operowac na tym nowym znakowaniu
                //newM=M+Teye[i]*Dmatrix, czyli nei trzeba nawet mnozyc macierzy
                Mcurrent = Helpers.AddElementWise(Mcurrent, DincidenceMatrix[CurrentlyCheckedTransition - 1]);
                //w Dincidence juz sa dodane Din i Dout
            }
            CurrentlyCheckedTransition++;
            if (CurrentlyCheckedTransition > NumberOfTransitions)
            {
                CurrentlyCheckedTransition = 1;
            }
            CurrentStep++;
        }


        private bool IsTransitionReadyToFire(List<int> DoutMatrixRow) //condition - i-ty wiersz macierzy Dout
        {
            //tu jest int zamiast bool bo kopiowalem z Cpp ktore robilem na arduino
            bool result = true;
            bool check = true;
            for (int i = 0; i < NumberOfPlaces; i++) //czy warunek spelniony dla wszystkich miejsc
            {
                //teraz dla kazdego miejsca sprawdzamy czy caly warunek jest spelniony
                //czy liczba znacznikow w M jest >= od wymaganych w tranzycji
                if (Mcurrent[i] < DoutMatrixRow[i])
                {
                    check = false;
                }
            }
            return result && check;
        }

        public void FIllWithExampleData()
        {
            NumberOfPlaces = 4;
            NumberOfTransitions = 3;
            CurrentStep = 0;
            CurrentlyCheckedTransition = 1; //bo se licze normlanie, do wierszy ja zmniejsze
            Mbegin = new List<int>(new[] {1, 0, 0, 0});
            Mcurrent = Mbegin;

            DinMatrix = new List<List<int>>();
            DinMatrix.Add(Helpers.CreateList(0, 1, 1, 0));
            DinMatrix.Add(Helpers.CreateList(0, 0, 0, 1));
            DinMatrix.Add(Helpers.CreateList(1, 0, 0, 0));

            DoutMatrix = new List<List<int>>();
            DoutMatrix.Add(Helpers.CreateList(1, 0, 0, 0));
            DoutMatrix.Add(Helpers.CreateList(0, 1, 1, 0));
            DoutMatrix.Add(Helpers.CreateList(0, 0, 0, 1));

            MakeIncidenceMatrix();
        }

        public void RestartPetriNet()
        {
            CurrentStep = 0;
            CurrentlyCheckedTransition = 1; //bo se licze normlanie, do wierszy ja zmniejsze
            //nowy Mbegin
            List<int> lst = new List<int>();
            for (int i = 0; i < NumberOfPlaces; i++)
            {
                if (i < Mbegin.Count)
                {
                    lst.Add(Mbegin[i]); //przepisuje stare wartisci ile sie zmiesci
                }
                else
                {
                    lst.Add(0); //dopleniam zerami
                }
            }
            Mbegin = lst; //nowy Mbegin
            Mcurrent = Mbegin;

            //nowe macierze Din i Dout
            DinMatrix = Helpers.ResizeMatrix(DinMatrix, DinMatrix.Count, DinMatrix[0].Count, NumberOfTransitions,
                NumberOfPlaces);
            DoutMatrix = Helpers.ResizeMatrix(DoutMatrix, DoutMatrix.Count, DoutMatrix[0].Count, NumberOfTransitions,
                NumberOfPlaces);
            MakeIncidenceMatrix();
        }

        public void FIllAllWithZeros()
        {
            //zrob
            CurrentStep = 0;
            CurrentlyCheckedTransition = 1; //bo se licze normlanie, do wierszy ja zmniejsze
            List<List<int>> lst=new List<List<int>>();
            lst.Add(Mbegin);
            lst = Helpers.FillWithZeros(lst);//a zeby ni etrworzyc kolejnej metody
            Mbegin = lst[0];
            DinMatrix = Helpers.FillWithZeros(DinMatrix);
            DoutMatrix = Helpers.FillWithZeros(DoutMatrix);

            MakeIncidenceMatrix();
        }

    }
}
