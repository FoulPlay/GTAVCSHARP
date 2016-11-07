using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using GTA;
using GTA.Native;

//Just a Cop spawner script for testing the AI | By Foul Play (Nathan Binks) | Version 1.1.0 TEST

namespace CopSpawner
{
	public class Main : Script
	{
		public Main()
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
				//CreateCop(); //Just for testing, it creates a cop
				CreateCopsandVehicle(); //Creates a cop car and cops in it
			}
			if (e.KeyCode == Keys.D9)
			{
				ChangeCOPRL(); //Change the relation groups between the gangs and cops
			}
			if (e.KeyCode == Keys.D8)
			{
				CreateSwatandVehicle();
			}
		}

		void ChangeCOPRL()
		{
			//Set the relation groups to dislike for possible war
			World.SetRelationshipBetweenGroups(Relationship.Dislike, Function.Call<int>(Hash.GET_HASH_KEY, "COP"), Function.Call<int>(Hash.GET_HASH_KEY, "AMBIENT_GANG_FAMILY"));
			World.SetRelationshipBetweenGroups(Relationship.Dislike, Function.Call<int>(Hash.GET_HASH_KEY, "COP"), Function.Call<int>(Hash.GET_HASH_KEY, "AMBIENT_GANG_BALLAS"));
		}

		void CreateSwatandVehicle()
		{
			//Creates the swat armored truck (The model (HASH), where it spawns in the world (VECTOR 3))
			Vehicle swatVeh = World.CreateVehicle(VehicleHash.Dilettante, Game.Player.Character.GetOffsetInWorldCoords(new GTA.Math.Vector3(0, 10, 0)));
			swatVeh.MarkAsNoLongerNeeded();
			swatVeh.PlaceOnGround();
			//Creats the driver and passengers (seat, model (HASH))
			Ped swatDriver = swatVeh.CreatePedOnSeat(VehicleSeat.Driver, PedHash.Cop01SFY);
			Ped swatPassengerRightFront = swatVeh.CreatePedOnSeat(VehicleSeat.RightFront, PedHash.Cop01SMY);
			Ped swatPassengerLeftRear = swatVeh.CreatePedOnSeat(VehicleSeat.LeftRear, PedHash.Cop01SMY);
			Ped swatPassengerRightRear = swatVeh.CreatePedOnSeat(VehicleSeat.RightRear, PedHash.Cop01SFY);
			//Give the driver and passengers weapons (Weapon (HASH), ammo, equp the weapon, is ammo loaded in clip)
			swatDriver.Weapons.Give(WeaponHash.CombatPDW, 500, false, false);
			swatDriver.Weapons.Give(WeaponHash.CombatPistol, 500, false, false);
			swatPassengerRightFront.Weapons.Give(WeaponHash.CombatPDW, 500, false, false);
			swatPassengerRightFront.Weapons.Give(WeaponHash.CombatPistol, 500, false, false);
			swatPassengerLeftRear.Weapons.Give(WeaponHash.CombatPDW, 500, false, false);
			swatPassengerLeftRear.Weapons.Give(WeaponHash.CombatPistol, 500, false, false);
			swatPassengerRightRear.Weapons.Give(WeaponHash.PumpShotgun, 500, false, false);
			swatPassengerRightRear.Weapons.Give(WeaponHash.CombatPistol, 500, false, false);
			//Marks them "no longer needed" them so GTA V AI system takes over them
			swatDriver.MarkAsNoLongerNeeded();
			swatPassengerRightFront.MarkAsNoLongerNeeded();
			swatPassengerLeftRear.MarkAsNoLongerNeeded();
			swatPassengerRightRear.MarkAsNoLongerNeeded();
		}

		void CreateCopsandVehicle()
		{
			//Creates the cop car (The model (HASH), where it spawns in the world (VECTOR 3))
			Vehicle copVeh = World.CreateVehicle(VehicleHash.Police2, Game.Player.Character.GetOffsetInWorldCoords(new GTA.Math.Vector3(0, 10, 0)));
			//Place it on the ground
			copVeh.PlaceOnGround();
			copVeh.MarkAsNoLongerNeeded();
			//Creates the driver (seat, model (HASH))
			Ped copDriver = copVeh.CreatePedOnSeat(VehicleSeat.Driver, PedHash.Cop01SMY);
			//Creates the passenger (seat, model (HASH))
			Ped copPassenger = copVeh.CreatePedOnSeat(VehicleSeat.Passenger, PedHash.Cop01SMY);
			//Give the driver and passenger weapons (Weapon (HASH), ammo, equp the weapon, is ammo loaded in clip)
			copDriver.Weapons.Give(WeaponHash.Pistol, 500, false, false);
			copPassenger.Weapons.Give(WeaponHash.Pistol, 500, false, false);
			copPassenger.Weapons.Give(WeaponHash.CarbineRifle, 500, false, false);
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