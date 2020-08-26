/*----------------------------------------------------------------------------
 * Copyright Meredith F Purk II 2018
 * sceneCollision - Contains functions and declarations for collision and scene/draw layers
----------------------------------------------------------------------------*/

// Scene Layers - These layers are for drawing only (not collision)

    SomeDudeConcept.BackgroundFar = 29; // Background layers (things in the very background)
    SomeDudeConcept.BackgroundMid = 28;
    SomeDudeConcept.BackgroundNear = 27;

    SomeDudeConcept.MidgroundFar = 26; // Midground layers (things behind the truck, but in front of the background layers
    SomeDudeConcept.MidgroundMid = 25;
    SomeDudeConcept.MidgroundNear = 24;

    /*
     * Example: House Rooms: layer 26
     *          House Walls: layer 25
     */
    
    SomeDudeConcept.TruckBedFar = 21; // Object-in-Truck (things in the truck bed. Appear in front of midground objects, but behind the truckbody and decals)
    SomeDudeConcept.TruckBedMid = 20;
    SomeDudeConcept.TruckBedNear = 19;

    SomeDudeConcept.TruckImage = 18; // Truck Body
    SomeDudeConcept.TruckDecals = 17; // Any decals/overlays we want to put on the truck (will appear in front of the truck, but behind objects, wheels, etc.)

    SomeDudeConcept.Wheel = 15; // Wheels and objects reside in these layers
    SomeDudeConcept.WheelDecals = 14; // Decals/details/overlays for the wheels
    SomeDudeConcept.ObjectDomain = 13; // Objects out of the truck bed/on the ground, can be hit by wheels, etc.
    SomeDudeConcept.ObjectDecals = 12; // Decals/overlays for objects

    SomeDudeConcept.HouseRooms = 11; // Rooms of houses
    SomeDudeConcept.HouseWalls = 10; // Walls of houses

    SomeDudeConcept.ObjectPreLoad = 9; // OBjects at rest in houses before any loading or manipulation
    SomeDudeConcept.ObjectManipulated = 8; // Objects being manipulated, but not moved into the truck yet. Once they collide with the truck bed they will be moved to the proper truck bed layer

    SomeDudeConcept.ForegroundFar = 6; // Foreground layers
    SomeDudeConcept.ForegroundMid = 5;
    SomeDudeConcept.ForegroundNear = 4;

    SomeDudeConcept.InterfaceFar = 3; // Interface/GUI layers
    SomeDudeConcept.InterfaceMid = 2;
    SomeDudeConcept.InterfaceNear = 1;



// Scene Groups    -   These layers are used for collision only (values are different for scene layers)
        
        // The Truck
        //SomeDudeConcept.ColTruckObjectDomain+1 = 21
        SomeDudeConcept.ColTruckObjectDomain = 20; // Includes values above and below it for three 'rows' in the truck bed
        //SomeDudeConcept.ColTruckObjectDomain-1 = 19
      
        SomeDudeConcept.ColTruckBodyDomain = 18;
        
       
        // Ground/Objects/Objects/Wheels
        SomeDudeConcept.ColGroundDomain = 11;
        SomeDudeConcept.ColObstacleDomain = 10;
        SomeDudeConcept.ColWheelDomain = 9;
        SomeDudeConcept.ColObjectDomain = 8;

//------------------------------------------------------------------
// Functions for collision

function SomeDudeConcept::updateCollision(%this, %object, %compositeObject)
{
if (%compositeObject == true)
    {
    %object.other.setSceneLayer(SomeDudeConcept.TruckBedMid+%object.other.currentRow);
    %object.setSceneLayer(SomeDudeConcept.TruckBedMid+%object.currentRow);
    %object.other.setCollisionGroups(SomeDudeConcept.ColTruckObjectDomain+%object.other.currentRow, SomeDudeConcept.ColTruckBodyDomain,  SomeDudeConcept.ColGroundDomain, SomeDudeConcept.ColObstacleDomain, SomeDudeConcept.ColObjectDomain); // Remove the wheel domain for objects in the truck bed
    %object.setSceneGroup(SomeDudeConcept.ColTruckObjectDomain+%object.currentRow);
    %object.other.setCollisionGroups(SomeDudeConcept.ColTruckObjectDomain+%object.other.currentRow, SomeDudeConcept.ColTruckBodyDomain,  SomeDudeConcept.ColGroundDomain, SomeDudeConcept.ColObstacleDomain, SomeDudeConcept.ColObjectDomain); // Remove the wheel domain for objects in the truck bed
    %object.setSceneGroup(SomeDudeConcept.ColTruckObjectDomain+%object.currentRow);
    }
else
    {
    %object.setSceneLayer(SomeDudeConcept.TruckBedMid+%object.currentRow); // Adjust the scene layer between Far, Mid, and Near based on its row
    %object.setCollisionGroups(SomeDudeConcept.ColTruckObjectDomain+%object.currentRow, SomeDudeConcept.ColTruckBodyDomain, SomeDudeConcept.ColGroundDomain, SomeDudeConcept.ColObstacleDomain, SomeDudeConcept.ColObjectDomain); // Remove the wheel domain for objects in the truck bed
    %object.setSceneGroup(SomeDudeConcept.ColTruckObjectDomain+%object.currentRow);
    }
}

function SomeDudeConcept::restoreCollision(%this, %object, %compositeObject)
{
if (%sompositeObject == true)
    {
    %object.other.setSceneLayer(SomeDudeConcept.ObjectDomain);
    %object.setSceneLayer(SomeDudeConcept.ObjectDomain);
    %object.other.setCollisionGroups(SomeDudeConcept.ColGroundDomain, SomeDudeConcept.ColObstacleDomain, SomeDudeConcept.ColWheelDomain, SomeDudeConcept.ColObjectDomain, SomeDudeConcept.ColTruckBodyDomain); // restore wheel domain to collision
    %object.other.setSceneGroup(SomeDudeConcept.ColObjectDomain);
    %object.setCollisionGroups(SomeDudeConcept.ColGroundDomain, SomeDudeConcept.ColObstacleDomain, SomeDudeConcept.ColWheelDomain, SomeDudeConcept.ColObjectDomain, SomeDudeConcept.ColTruckBodyDomain); // restore wheel domain to collision
    %object.setSceneGroup(SomeDudeConcept.ColObjectDomain);
    }
else
    {
    %object.setSceneLayer(SomeDudeConcept.ObjectDomain); // Adjust the scene layer
    %object.setCollisionGroups(SomeDudeConcept.ColGroundDomain, SomeDudeConcept.ColObstacleDomain, SomeDudeConcept.ColWheelDomain, SomeDudeConcept.ColObjectDomain, SomeDudeConcept.ColTruckBodyDomain);  // Restore wheel domain to collision
    %object.setSceneGroup(SomeDudeConcept.ColObjectDomain);
    }
}

function TruckBody::onCollision(%this, %object, %collisionDetails)
{ 
    if (%this.getCollisionShapeIsSensor(getWord(%collisionDetails, 0)) == true)
        {
        %object.inTruck = true;
        // Is this during the loading phase?
        if (SomeDudeConcept.LoadPhase <= 1)
            { // Truck is being loaded, set the object to the proper row. We do not need to save old collision data
            if (%object.class $= "grandpa")
                { // Make sure both halves of grandpa behave the same
                %object.other.currentRow = SomeDudeConcept.LoadPhase;
                %object.currentRow = SomeDudeConcept.LoadPhase;
                }
            else
                {
                %object.currentRow = SomeDudeConcept.LoadPhase;
                }
            }             
        if (%object.compositeObject == true)
            { // Make sure both halves of the object behave the same
            SomeDudeConcept.updateCollision(%object, true);
            }
        else
            {
            SomeDudeConcept.updateCollision(%object, false);
            }
        }
    else
        return; // break out if we aren't touching a sensor
}

function TruckBody::onEndCollision(%this, %object, %collisionDetails)
{
    // We only care if the object colliding is the sensor in the truck bed
    if (%this.getCollisionShapeIsSensor(getWord(%collisionDetails, 0)) == true)
        {
        %object.inTruck = false;
       
        if (%object.compositeObject == true)
            { // Make sure both halves behave the same
            if (%object.other.inTruck == false)
                { 
                SomeDudeConcept.restoreCollision(%object, true);
                }
            }
        else
            {
            SomeDudeConcept.restoreCollision(%object, false);
            }        
        }
    else
        return; // break out if we aren't touching a sensor
}

function EndZone::onCollision(%this, %object, %collisionDetails)
{
    // Check if the truck is colliding with the end zone
    if (%object.class $= "TruckBody")
        {
        if (SomeDudeConcept.endGame < 1)
            {
            SomeDudeConcept.endGame = 1; // Game is over
            SomeDudeConcept.allowTruckDriving = false; // Turn off truck driving
            echo("SomeDude has reached the end zone!");
            // turn off the wheel/driving motion to let the truck come to a stop
            truckReverse(false);
            truckForward(true, 0.5);
            SomeDudeConcept.endDriveTime = SandboxScene.getSceneTime(); // Record the ending time for the simulation
            SomeDudeConcept.schedule(3000, "truckStop");
            SomeDudeConcept.endGame = new TextSprite()
                                {
                                Scene = SandboxScene;
                                Font = "ToyAssets:TrajanProFont";
                                FontSize = 1;
                                Text = "FINISH!";
                                Position = SomeDudeConcept.truckBody.getPositionX() SPC SomeDudeConcept.truckBody.getPositionY()+1;
                                Size = "90 7";
                                OverflowModeX = "visible";
                                TextAlignment = "center";
                                BlendColor = "blue";
                                GravityScale = "0.1";
                                };
            SomeDudeConcept.schedule(3000, "getObjectsDelivered"); // Gather the objects delivered
            // Both of the functions above create an array of objects either in the end zone or not in the end zone
            SomeDudeConcept.schedule(5000, "showStatsScreen"); // Display the stats
            
            }
        
        }
    return;
}