using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using System.Linq;
namespace DemoSchedulerAgent
{
    public class TaskScheduler : ScheduledTaskAgent
    {
        /// <summary>
        /// Agent that runs a scheduled task
        /// </summary>
        /// <param name="task">
        /// The invoked task
        /// </param>
        /// <remarks>
        /// This method is called when a periodic or resource intensive task is invoked
        /// </remarks>
        protected override void OnInvoke(ScheduledTask task)
        {

            //TODO: Add code to perform your task in background

            /// If application uses both PeriodicTask and ResourceIntensiveTask
            if (task is PeriodicTask)
            {
                // Execute periodic task actions here.
                ShellTile TileToFind = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("TileID=2"));
                if (TileToFind != null)
                {
                     StandardTileData NewTileData = new StandardTileData
                        {
                            Title= "updated by scheduled task",
                            Count = System.DateTime.Now.Minute
                        };
                     TileToFind.Update(NewTileData);
                }
            }
            else
            {
                // Execute resource-intensive task actions here.
            }

            NotifyComplete();
           
        }

        /// <summary>
        /// Called when the agent request is getting cancelled
        /// </summary>
        protected override void OnCancel()
        {
            base.OnCancel();

            //TODO: This Task is cancelled, do necessary clean up 
        }
    }
}
