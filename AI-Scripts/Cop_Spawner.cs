using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using GTA;
using GTA.Native;

//Just a Cop spawner script for testing the AI | By Foul Play (Nathan Binks) | Version 1.0.0 TEST

namespace ClassLibrary1
{
    public class Class1 : Script
    {
		public Class1()
		{
			this.Tick += onTick;
			this.KeyDown += onKeyDown;
			this.KeyUp += onKeyUp;
		}

		private void onTick(object sender, EventArgs e)
		{
		}

		private void onKeyDown(object sender, KeyEventArgs e)
		{
		}

		private void onKeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.D0)
			{
				//ChangeCOPRL(); // Change the relation groups between the gangs and cops
				//CreateCop(); // Just for testing, it creates a cop
				CreateCopsandCar(); // Creates a cop car and cops in it
			}
		}

		void ChangeCOPRL()
		{
			//Set the relation groups to dislike for possible war
			World.SetRelationshipBetweenGroups(Relationship.Dislike, Function.Call<int>(Hash.GET_HASH_KEY, "COP"), Function.Call<int>(Hash.GET_HASH_KEY, "AMBIENT_GANG_FAMILY"));
			World.SetRelationshipBetweenGroups(Relationship.Dislike, Function.Call<int>(Hash.GET_HASH_KEY, "COP"), Function.Call<int>(Hash.GET_HASH_KEY, "AMBIENT_GANG_BALLAS"));
		}

		void CreateCopsandCar()
		{
			//Creates the cop car (The model (HASH), where it spawns in the world (VECTOR 3))
			Vehicle copVeh = World.CreateVehicle(VehicleHash.Police, Game.Player.Character.GetOffsetInWorldCoords(new GTA.Math.Vector3(0, 5, 0)));
			//Creates the driver (seat, model (HASH))
			Ped copDriver = copVeh.CreatePedOnSeat(VehicleSeat.Driver, PedHash.Cop01SMY);
			//Creates the passenger (seat, model (HASH))
			Ped copPassenger = copVeh.CreatePedOnSeat(VehicleSeat.Passenger, PedHash.Cop01SMY);
			//Give the driver and passenger weapons (Weapon (HASH), ammo, equp the weapon, is ammo loaded in clip)
			copDriver.Weapons.Give(WeaponHash.Pistol, 500, false, false);
			copPassenger.Weapons.Give(WeaponHash.Pistol, 500, false, false);
			copPassenger.Weapons.Give(WeaponHash.AdvancedRifle, 500, false, false);
			//Marks them "no longer needed" them so GTA V AI system takes over them
			copVeh.MarkAsNoLongerNeeded();
			copDriver.MarkAsNoLongerNeeded();
			copPassenger.MarkAsNoLongerNeeded();
		}

		void CreateCop()
		{
			//Create the cop (model (HASH), where the ped spawns in the world (VECTOR 3))
			Ped cop = World.CreatePed(PedHash.Cop01SMY, Game.Player.Character.GetOffsetInWorldCoords(new GTA.Math.Vector3(0, 5, 0)));
			//Give the cop weapons (Weapon (HASH), ammo, equp the weapon, is ammo loaded in clip)
			cop.Weapons.Give(WeaponHash.SMG, 500, true, false);
			cop.Weapons.Give(WeaponHash.Pistol, 500, false, false);
			//Get the "COP" hash for setting relation group of the ped
			int copRLGHash = Function.Call<int>(Hash.GET_HASH_KEY, "COP");
			//Set the ped relation group to "COP"
			Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, copRLGHash);

			//Tesing to make them not die when getting shot in cerin body parts
			Function.Call(Hash.SET_PED_SUFFERS_CRITICAL_HITS, cop, false);
			Function.Call(Hash.SET_PED_DIES_WHEN_INJURED, cop, false);
			Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, cop, 26, true);
			//Make sure the ped doesn't die on low health
			cop.AlwaysDiesOnLowHealth = false;
			//Marks the ped "no longer needed" so GTA V AI system takes over it
			cop.MarkAsNoLongerNeeded();
		}
	}
}
