using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GTA;

namespace KillerOnTheLooseV
{
    //By Nathan Binks version 1.0.0 development build
    public class main : Script
    {
        Ped killer;
        public main()
        {
            this.KeyUp += this.OnKeyUp;
            this.Tick += this.OnTick;
            killer = null;
        }

        //Checks if the killer exists
        bool killerExists()
        {
            if (killer != null && killer.Exists()) return true; else return false;
        }

        //Killer will attack people
        void attackPedestrians()
        {
            if (killerExists())
            {
                //Get nerby peds in world at killers position in 100 feet (I think)
                foreach (Ped ped in World.GetNearbyPeds(killer.Position, 100f))
                {
                    if (!ped.Exists()) continue; //If the ped doesn't exist then continue
                    if (ped == Game.Player.Character) continue; //If the ped is the player then continue
                    if (ped == killer) continue; //If the ped is the killer then continue
                    if (killer.IsInCombat) continue; //If the killer is already attacking a ped then continue
                    if (ped.IsInVehicle() && ped.CurrentVehicle.Speed > 10f) continue; //If the ped is in a vehicle and the speed is over 10 MPH then continue
                    attackPed(ped); //Attack the ped
                }
            }
        }

        void attackPed(Ped ped)
        {
            if (killerExists())
            {
                killer.Task.FightAgainst(ped); //Attack the ped
            }
        }

        void spawnKiller()
        {
            if (killerExists()) return;

            killer = World.CreateRandomPed(Game.Player.Character.Position); //Create the killer
            killerProperties(); //Set killer's Properties
            giveWeapon(); //Give the killer a weapon
        }

        void killerProperties()
        {
            if(killerExists())
            {
                killer.BlockPermanentEvents = true; //Don't be distracted
                killer.CanSufferCriticalHits = false; //Don't be killed easily
                killer.AlwaysKeepTask = true; //Always keep their task assigned
                killer.Accuracy = 95; //Make them able to hit any targets
                killer.Armor = 255; //Set the armor to 255
            }
        }

        void giveWeapon()
        {
            if (killerExists())
            {
                killer.Weapons.Give(GTA.Native.WeaponHash.SMG, 9999, true, false); //Give them a SMG
            }
        }

        private void OnKeyUp(Object sender, KeyEventArgs e)
        {
            if (e.Shift && e.KeyCode == Keys.D0)
            {
                for(int i = 0; i < 1; i++ ) spawnKiller();
            }
        }

        private void OnTick(Object sender, EventArgs e)
        {
            if (killerExists())
            {
               // attackPedestrians();
                if (killer.IsDead) killer.Delete(); killer = null; //If the killer is dead then delete and make killer null
            }
        }
    }
}
