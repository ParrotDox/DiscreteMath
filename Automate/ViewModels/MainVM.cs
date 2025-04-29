using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Automate.Core;

namespace Automate.ViewModels
{
    public class MainVM : ObservableObject
    {
        private enum State 
        {
            q0,
            qa,
            qb,
            qc,
            qd,
            qError
        }
        private bool? _result = true;
        public bool? Result 
        {
            get 
            {
                return _result;
            }
            set 
            {
                _result = value;
                OnPropertyChanged();
            }
        }
        private string? _input = null;
        public string? Input
        {
            get
            {
                return _input;
            }
            set
            {
                _input = value;
                Result = UpdateResult(_input);
                OnPropertyChanged();
            }
        }
        public ICommand PutRandomInputCommand { get; set; }
        public MainVM()
        {
            PutRandomInputCommand = new RelayCommand(PutRandomInput, null);
        }
        private void PutRandomInput(object? param) 
        {
            string[] options = { "ABCD", "BBCD", "ABDCACACDB", "BCDBACDBAA", "ABDCACBAB", "A", "B", "C", "D", "ABC", "BCA", "DAB", "AAA", "BBDD", "CAB", "BACAD", "DDCCAABB","DCBADCBADBADBABDC","DCBCACBD" };
            Random rnd = new Random();
            Input = options[rnd.Next(options.Length)];
        }
        //Automate methods
        private bool? UpdateResult(string? input) 
        {
            if (input == null) 
            {
                return true;
            }

            string formattedInput = input.ToLower();
            State currentState = State.q0;

            foreach(char c in formattedInput) 
            {
                switch (currentState) 
                {
                    case State.q0: 
                        {
                            currentState = GetNextState(c);
                            break;
                        }
                    case State.qa: 
                        {
                            currentState = c == 'a' ? State.qError : GetNextState(c);
                            break;
                        }
                    case State.qb:
                        {
                            currentState = c == 'b' ? State.qError : GetNextState(c);
                            break;
                        }
                    case State.qc:
                        {
                            currentState = c == 'c' ? State.qError : GetNextState(c);
                            break;
                        }
                    case State.qd:
                        {
                            currentState = c == 'd' ? State.qError : GetNextState(c);
                            break;
                        }
                    case State.qError:
                        {
                            break;
                        }
                }
                if (currentState == State.qError) { break; }
            }
            return currentState != State.qError;
        }
        private static State GetNextState(char c) 
        {
            switch (c) 
            {
                case 'a':
                    {
                        return State.qa;
                    }
                case 'b':
                    {
                        return State.qb;
                    }
                case 'c':
                    {
                        return State.qc;
                    }
                case 'd':
                    {
                        return State.qd;
                    }
            }
            return State.qError;
        }
    }
}
