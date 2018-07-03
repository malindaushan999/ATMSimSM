using StateManager.States;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace StateManager
{
    public class ATMSimStateManager
    {
        /// <summary>
        /// General queue that the commands are added
        /// </summary>
        public static Queue<ATMSimCommand> GeneralQueue = new Queue<ATMSimCommand>();

        private BackgroundWorker backgraoundWorker;

        public ATMSimStateManager()
        {
            backgraoundWorker = new BackgroundWorker();
            backgraoundWorker.DoWork += StartStateManager_DoWork;
            backgraoundWorker.RunWorkerAsync();
        }
        
        public void StartStateManager_DoWork(object sender, DoWorkEventArgs e)
        {
            StateHolder.CurrentState = new IdleState();
            StateHolder.CurrentState.Enter();

            while (true)
            {
                try
                {
                    if(GeneralQueue.Count != 0)
                    {
                        ATMSimCommand command = GeneralQueue.Dequeue();                                    // remove first command from queue output
                        Console.WriteLine(String.Concat("\nGENERAL Q CMD: ", command.CommandId));
                        if (command != null)
                        {
                            Result doProcessState = StateHolder.CurrentState.DoProcess(command);           // run doProcess() of current state with the command

                            if (doProcessState.Equals(Result.Success))
                            {
                                StateHolder.NextState = StateHolder.CurrentState.GetNextState(command);    // assign current state's next state as next state
                                if (StateHolder.NextState != null)
                                {
                                    StateHolder.PreviousState = StateHolder.CurrentState;                  // assign current state as previous state
                                    StateHolder.CurrentState.Exit();                                       // exit from current state
                                    StateHolder.CurrentState = StateHolder.NextState;                      // assign next state as current state
                                    StateHolder.NextState = null;                                          // set next state as null
                                    StateHolder.CurrentState.Enter();                                      // enter to current state
                                }
                            }
                            else if (doProcessState.Equals(Result.GoBack))
                            {
                                var temp = StateHolder.CurrentState;
                                StateHolder.CurrentState.Exit();
                                StateHolder.PreviousState.Enter();
                                StateHolder.CurrentState = StateHolder.PreviousState;
                                StateHolder.PreviousState = temp;
                            }

                            Console.WriteLine(String.Concat("\nSTATE TRANSITION: ", StateHolder.PreviousState, " --> ", StateHolder.CurrentState));
                            Console.WriteLine(String.Concat("CURRENT STATE: ", StateHolder.CurrentState));
                        }
                        Thread.Sleep(100);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(String.Concat("ERROR STATE MGR: ", ex.Message));
                }
            }
        }

        /// <summary>
        /// Add commands to th queue
        /// </summary>
        /// <param name="command">command</param>
        public static void AddToQueue(ATMSimCommand command)
        {
            GeneralQueue.Enqueue(command);
        }
    }
}
