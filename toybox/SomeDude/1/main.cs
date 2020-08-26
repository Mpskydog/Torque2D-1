//-----------------------------------------------------------------------------
// Copyright (c) 2013 GarageGames, LLC
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
//-----------------------------------------------------------------------------
exec("./sceneCollision.cs");
exec("./editors/editors.cs");
exec("./constants.cs");

function SomeDudeConcept::endLevel(%this)
{
    // Ending level
    echo("ending level");
    SomeDudeConcept.rearWheel.setGatherContacts(false);
    SomeDudeConcept.frontWheel.setGatherContacts(false);
    %this.reset();
}

function SomeDudeConcept::create( %this )
{ 
    
    ReleaseBuildInfo.setText(SomeDudeConcept.GameRelease SPC SomeDudeConcept.GameBuild);

    // On Level Start - we allow pan/pull availability at first, because we are letting the player manipulate objects to load the truck
    // Set the sandbox drag mode availability.
    Sandbox.allowManipulation( pan, false );
    Sandbox.allowManipulation( pull, false);
    SomeDudeConcept.allowTruckDriving = false;      
    // Reset the toy.
    %this.reset();
}

// -----------------------------------------------------------------------------

function SomeDudeConcept::createTimer(%this)
{
    %this.countDownTimer = new TextSprite()
                                {
                                Scene = SandboxScene;
                                Font = "ToyAssets:TrajanProFont";
                                FontSize = 1;
                                Text = "LOAD THAT TRUCK..!";
                                Position = %this.truckBody.getPositionX() SPC %this.truckBody.getPositionY()+1;
                                Size = "90 7";
                                OverflowModeX = "visible";
                                TextAlignment = "center";
                                BlendColor = "sea green";
                                GravityScale = "";
                                };
    %this.loadPhase = 1;
    %this.loadPhaseCountdown = 10;
    %this.startTimer("startLoadPhase", 1000);
    %this.instructionGUI = new Sprite()
                            {
                            Scene = SandboxScene;
                            Position = %this.truckBody.getPositionX() SPC %this.truckBody.getPositionY()-3.5;
                            Size = "3 3";
                            Image = "SomeDudeConcept:truckBedImage";
                            Frame = "0";
                            GravityScale = "";
                            };
    Sandbox.setObjectNoManipulation(%this.countDownTimer, true);
    Sandbox.setObjectNoManipulation(%this.instructionGUI, true);
}

function SomeDudeConcept::startLoadPhase(%this)
{
    // Run by the createTimer function
    if (SomeDudeConcept.loadPhaseCountdown > 0)
        {
        SomeDudeConcept.TruckBody.setBlendAlpha(0.8); // This helps us see the objects in the truck as we load them/move it
        //SomeDudeConcept.TruckBody.setBodyType("static"); // Set the truck so it can't be interacted with/moved during the load phase
        SomeDudeConcept.FrontWheel.setBodyType("static");
        SomeDudeConcept.RearWheel.setBodyType("static");
        SomeDudeConcept.loadPhaseCountdown = SomeDudeConcept.loadPhaseCountdown-1;
        SomeDudeConcept.countDownTimer.Text = SomeDudeConcept.loadPhaseCountdown; // Update the text
        if (SomeDudeConcept.loadPhaseCountdown < 5)
            SomeDudeConcept.countDownTimer.BlendColor = "red";
        }
    else
        {
        if (SomeDudeConcept.loadPhase == 1)
            {
            SomeDudeConcept.loadPhaseCountdown = 10;
            SomeDudeConcept.loadPhase = 0;
            SomeDudeConcept.instructionGUI.setImageFrame("1");
            SomeDudeConcept.countDownTimer.BlendColor = "sea green";
            SomeDudeConcept.countDownTimer.Text = SomeDudeConcept.loadPhaseCountdown;
            }
        else if (SomeDudeConcept.loadPhase == 0)
            {
            SomeDudeConcept.loadPhaseCountdown = 10;
            SomeDudeConcept.loadPhase = -1;
            SomeDudeConcept.instructionGUI.setImageFrame("2");
            SomeDudeConcept.countDownTimer.BlendColor = "sea green";
            SomeDudeConcept.countDownTimer.Text = SomeDudeConcept.loadPhaseCountdown;
            }
        else if (SomeDudeConcept.loadPhase == -1)
            {
            SomeDudeConcept.stopTimer();   
            SomeDudeConcept.countDownTimer.delete();
            SomeDudeConcept.instructionGUI.delete();
            Sandbox.useManipulation(Off);
            
            if (SomeDudeConcept.currentObject !$= "")
                { // Make sure we clear the object manipulation we were working with
                Sandbox.ManipulationPullObject[SomeDudeConcept.currentObject] = "";
                SandboxScene.deleteJoint( Sandbox.ManipulationPullJointId[SomeDudeConcept.currentObject] );
                Sandbox.ManipulationPullJointId[SomeDudeConcept.currentObject] = "";
                SomeDudeConcept.currentObject = ""; // clear the object touch ID
                }

            SomeDudeConcept.loadPhase = 2;
            SomeDudeConcept.allowTruckDriving = true;
            SomeDudeConcept.TruckBody.setBlendAlpha(1);
            //SomeDudeConcept.TruckBody.setBodyType("dynamic"); // Set truck back to dynamic body
            SomeDudeConcept.FrontWheel.setBodyType("dynamic");
            SomeDudeConcept.RearWheel.setBodyType("dynamic");
            SomeDudeConcept.goDrive = new TextSprite()
                                {
                                Scene = SandboxScene;
                                Font = "ToyAssets:TrajanProFont";
                                FontSize = 1;
                                Text = "DRIVE..!";
                                Position = %this.truckBody.getPositionX() SPC %this.truckBody.getPositionY()+1;
                                Size = "90 7";
                                OverflowModeX = "visible";
                                TextAlignment = "center";
                                BlendColor = "green";
                                GravityScale = "0.1";
                                };
            SomeDudeConcept.FrontWheel.setGatherContacts(true); // Set wheels to gather contacts
            SomeDudeConcept.RearWheel.setGatherContacts(true);
            SomeDudeConcept.schedule(3000, "startDrivePhase");
            
            }
        }
}

//-----------------------------------------------------------------------------

function SomeDudeConcept::startDrivePhase(%this)
{ // Drive phase has begun, perform some cleanup and start the OnSceneUpdate calls
SomeDudeConcept.goDrive.delete(); // Remove the old 'drive!' sprite
//SandboxScene.UpdateCallback = true; // activate the onSceneUpdate call
SomeDudeConcept.startDriveTime = SandboxScene.getSceneTime(); // Record when the driving started

//SomeDudeConcept.TruckBody.setUpdateCallback(true); //Debug - checking velocity information
}

function TruckBody::onUpdate(%this)
{
echo(%this.getLinearVelocity());
}

function SomeDudeConcept::destroy( %this )
{
    // Deactivate the package.
    deactivatePackage( SomeDudeConceptPackage );
}

//-----------------------------------------------------------------------------

function SomeDudeConcept::reportContacts()
{
    %front = SomeDudeConcept.FrontWheel;
    %rear = SomeDudeConcept.RearWheel;
    %contFront = "Front:";
    %contRear = "Rear:";

    %count = 0;
    while (%count < %front.getContactCount())
        {
        %contFront = %contFront SPC getWord(%front.getContact(%count), 0);
        %count++;
        }

    %count = 0;
    while (%count < %rear.getContactCount())
        {
        %contRear = %contRear SPC getWord(%rear.getContact(%count), 0);
        %count++;
        }

    echo(%contFront);
    echo(%contRear);
}

function SomeDudeConcept::reset( %this )
{   
    // Clear the scene.
    SandboxScene.clear();    
    
    // Set a typical Earth gravity.
    SandboxScene.setGravity(SomeDudeConcept.GravityX, SomeDudeConcept.GravityY);  
    SandboxScene.setVelocityIterations(SomeDudeConcept.VelocityIterations);
    SandboxScene.setPositionIterations(SomeDudeConcept.PositionIterations);

    // Camera Configuration
    SandboxWindow.setCameraPosition( SomeDudeConcept.WorldLeft + (SomeDudeConcept.CameraWidth/2) - 10, 0 );
    SandboxWindow.setCameraAngle(SomeDudeConcept.CameraAngle);
    SandboxWindow.setCameraSize( SomeDudeConcept.CameraWidth, SomeDudeConcept.CameraHeight );
    //SandboxWindow.setViewLimitOn( SomeDudeConcept.WorldLeft, SomeDudeConcept.CameraHeight/-2, SomeDudeConcept.WorldRight, SomeDudeConcept.CameraHeight/2 );

    SomeDudeConcept.createEndzone();
    // Create the array(s) for holding objects
    SomeDudeConcept.createdObjects = new array();
    SomeDudeConcept.lostObjects = new array();
    SomeDudeConcept.deliveredObjects = new array();
    
    %this.createTilemap();
    if (SomeDudeConcept.Startup $= "tile_editor")
        %this.createTilemap_editor();

    // Create the scene contents in a roughly left to right order.   
    
    %this.createHouse();   

    // Background.
    %this.createBackground();

    // Floor and walls of the level
    %this.createFloor(0);    
    %this.createWalls();

    // Special startup modes
    if (SomeDudeConcept.StartUp $= "tile_editor")
        { // Tile Editor mode
        GlobalActionMap.bindCmd( keyboard, "up", "cameraUp();", "cameraStop();" );
        GlobalActionMap.bindCmd( keyboard, "down", "cameraDown();", "cameraStop();" );
        GlobalActionMap.bindCmd( keyboard, "left", "cameraLeft();", "cameraStop();" );
        GlobalActionMap.bindCmd( keyboard, "right", "cameraRight();", "cameraStop();" );
        GlobalActionMap.bindCmd( keyboard, "x", "flipTileX();", "");
        GlobalActionMap.bindCmd( keyboard, "y", "flipTileY();", "");
        GlobalActionMap.bindCmd( keyboard, "e", "rotateTileL();", "");
        GlobalActionMap.bindCmd( keyboard, "r", "rotateTileR();", "");
        }

    else if (SomeDudeConcept.StartUp $= "ground_editor")
        { // Ground Editor mode
        GlobalActionMap.bindCmd( keyboard, "up", "cameraUp();", "cameraStop();" );
        GlobalActionMap.bindCmd( keyboard, "down", "cameraDown();", "cameraStop();" );
        GlobalActionMap.bindCmd( keyboard, "left", "cameraLeft();", "cameraStop();" );
        GlobalActionMap.bindCmd( keyboard, "right", "cameraRight();", "cameraStop();" );
        }

    else if (SomeDudeConcept.StartUp $= "object_editor")
        { // Object Editor mode
        GlobalActionMap.bindCmd( keyboard, "up", "cameraUp();", "cameraStop();" );
        GlobalActionMap.bindCmd( keyboard, "down", "cameraDown();", "cameraStop();" );
        GlobalActionMap.bindCmd( keyboard, "left", "cameraLeft();", "cameraStop();" );
        GlobalActionMap.bindCmd( keyboard, "right", "cameraRight();", "cameraStop();" );
        GlobalActionMap.bindCmd( keyboard, "x", "flipSpriteX();", "");
        GlobalActionMap.bindCmd( keyboard, "y", "flipSpriteY();", "");
        GlobalActionMap.bindCmd( keyboard, "e", "rotateSpriteL();", "");
        GlobalActionMap.bindCmd( keyboard, "r", "rotateSpriteR();", "");
        }
    else
        {
        // If nothing special is needed (normal start up mode)
        %truckStartX = SomeDudeConcept.WorldLeft+1;
        %truckStartY = 3;   
        //%this.createTruck( -210, -2.5); 
        %this.createTruck( -210, 1.5); 
        //%this.createDriver(); // Create our driver
        //%this.createTrailer(-205, -2.5);
        TruckForward(true);
        SomeDudeConcept.TruckStop(); // This helps set the 'brakes' for the beginning
        
        %simSet = Tamlread("objects1.taml");
        SomeDudeConcept.moveObjectsToArray(%simset, SomeDudeConcept.createdObjects, true);        

        // Schedule load timer
        SomeDudeConcept.schedule(5000, "createTimer");
        // Set the manipulation mode.
        Sandbox.schedule(5000, "useManipulation", "pull");
        }
}

function SomeDudeConcept::createHouse(%this)
{
    %obj = Tamlread("house_rooms.taml");
    SandboxScene.add(%obj); // Add the house tiles to the level
    SandboxScene.loadHouse = %obj; // store the object for retrieval
    %obj = Tamlread("house_walls.taml");
    SandboxScene.add(%obj); // Add the wall tiles
    SandboxScene.loadWalls = %obj; // store the object for retrieval
}

// -----------------------------------------------------------------------------

function SomeDudeConcept::onRightMouseDown(%this, %touchID, %worldPosition)
{
    if (SomeDudeConcept.StartUp $= "ground_editor")
        { // Ground Collision editor
        SomeDudeConcept.popGroundPoint();
        echo("GroundCollisionRightDown");
        return; // Kick out now
        }
    else if (SomeDudeConcept.StartUp $= "tile_editor")
        {
        SomeDudeConcept.tilemap.selectSprite(%touchID, %worldPosition, 1); // right mouse
        echo("TileMapRightDown");
        return; // kick out
        }
}

function TileMapObject::onRightMouseDown(%this, %touchID, %worldPosition)
{
    if (SomeDudeConcept.StartUp $= "tile_editor")
        { // Tile editor
        %this.selectSprite(%touchID, %worldPosition, 1); // Right mouse
        echo("TileMapRightDown");
        }
}

function SomeDudeConcept::createTilemap(%this)
{
    %composite = tamlread("lvl1.taml");
    SandboxScene.add(%composite);
    SomeDudeConcept.Tilemap = %composite;
    SomeDudeConcept.Tilemap.allowManipulationPull = false;
    //SomeDudeConcept.Tilemap.setSceneLayer(28);
}

function SomeDudeConcept::createBackground(%this)
{  
    // Atmosphere
    %obj = new Sprite();
    %obj.setBodyType( "static" );
    %obj.setImage( "SomeDudeConcept:highlightBackground" );
    %obj.BlendColor = DarkGray;
    %obj.setSize( SomeDudeConcept.WorldWidth * (SomeDudeConcept.CameraWidth*2), 75 );
    %obj.setSceneLayer( SomeDudeConcept.BackgroundFar );
    %obj.setCollisionSuppress();
    %obj.setAwake( false );
    %obj.setActive( false );
    SandboxScene.add( %obj );  
}

// -----------------------------------------------------------------------------

function SomeDudeConcept::createWalls(%this)
{
    %obj = Tamlread("walls1.taml");
    SandboxScene.WallPlane = %obj; // store so we can retrieve later
    SandboxScene.add(%obj);
}

function SomeDudeConcept::createWalls_old(%this)
{
    // walls
    %obj = new Scroller();
    %obj.setBodyType( "static" );
    %obj.setImage( "ToyAssets:blank" );
    %obj.setBlendAlpha(0.0);
    %obj.setSize( SomeDudeConcept.WorldWidth, 3 );

    %obj.setPosition( 0, SomeDudeConcept.FloorLevel - (%obj.getSizeY()/2) );
    %obj.setSceneLayer( SomeDudeConcept.BackgroundMid);
    
    %obj.setRepeatX( SomeDudeConcept.WorldWidth / 12 );
    %obj.setDefaultFriction( SomeDudeConcept.ObstacleFriction );
    %obj.setSceneGroup(SomeDudeConcept.ColGroundDomain);
    %obj.setCollisionGroups( none );
    %obj.createEdgeCollisionShape( SomeDudeConcept.WorldLeft, -20, SomeDudeConcept.WorldLeft, 50 ); // Left Wall
    %obj.createEdgeCollisionShape( SomeDudeConcept.WorldRight, -20, SomeDudeConcept.WorldRight, 50 ); // Right Wall
    %obj.CollisionCallback = true;
    %obj.setAwake( false );
    SomeDudeConcept.WallPlanes = %obj; // Store the object so we can retrieve it later
    SandboxScene.add( %obj );   
}

function SomeDudeConcept::createFloor(%this)
{
    if (SomeDudeConcept.terrainGeneration == true)
        { // Generate dynamic terrain
        setRandomSeed(1); // Make sure we use a static 'random number generation' seed so we can reproduce the same results.
        SomeDudeConcept.initializeRandomTerrain();
        SomeDudeConcept.generateTerrain();
        }
    else
        { // Load pre-configured terrain file
        %obj = Tamlread("gnd1.taml");
        SomeDudeConcept.GroundPlane = %obj;
        SandboxScene.add(%obj); // Add the ground plane to the scene
        }
    
}

function SomeDudeConcept::createEndZone(%this)
{
    %obj = new Sprite();
    %obj.setBodyType("static");
    %obj.setImage("ToyAssets:blank");
    %obj.setBlendAlpha(0.0);
    %obj.setSize(0.5, 0.5);
    %obj.setSceneLayer(SomeDudeConcept.BackgroundFar);
    %obj.setPosition("209 -15.56");
    %obj.createPolygonBoxCollisionShape(24, 12);
    %obj.setCollisionShapeIsSensor(0, true);
    SandboxScene.add(%obj);
    SomeDudeConcept.endZone = %obj;
    %obj.class = "EndZone";
    %obj.setCollisionCallback(true);
    //203 -16.506
    //215 - 14.613
}

// ----------------------------------------------------------------------------
function SomeDudeConcept::getObjectsDelivered(%this)
{
    // Pick any objects in the area of the end zone
    %endZonePos1 = SomeDudeConcept.endZone.getPositionX()-12 SPC SomeDudeConcept.endZone.getPositionY()+6;
    %endZonePos2 = SomeDudeConcept.endZone.getPositionX()+12 SPC SomeDudeConcept.endZone.getPositionY()-6;
    %pickedDelivered = SandboxScene.pickArea(%endZonePos1, %endZonePos2, "", ""); // Pull a list of all objects in the end zone
    echo("Objects found in End Zone:" SPC %pickedDelivered);
    SomeDudeConcept.deliveredobjects.empty(); // Clear our array before working with it
    %count = 0;
    while (%count < getWordCount(%pickedDelivered))
        { // Go through the list of picked objects, compare them to what is in the created objects
        %object = getWord(%pickedDelivered, %count);
        echo("Working with array of picked objects:" SPC %count SPC %object);
        if (SomeDudeConcept.createdObjects.getIndexFromKey(%object) >-1)
            SomeDudeConcept.deliveredObjects.add(%object, 0); // Add to the delivered array
        %count++;
        }
    %this.getObjectsLost(); // Gather the objects lost on the roadway
}

function SomeDudeConcept::getObjectsLost(%this)
{
    // Copy anything not in the deliveredobjects to the lostobjects, and add the position X position as a value
    SomeDudeConcept.lostObjects.empty(); // Clear our array before working with it
    %count = 0;
    while (%count < SomeDudeConcept.createdObjects.count())
        {
        %object = SomeDudeConcept.createdObjects.getKey(%count);
        if (SomeDudeConcept.deliveredObjects.getIndexFromKey(%object) == -1)
            SomeDudeConcept.lostObjects.add(%object, %object.getPositionX()); // Add the object handle and X position to the lost objects
        %count++;
        }
    SomeDudeConcept.lostObjects.sortnd(); // Sort by the X position of the objects
}

function SomeDudeConcept::showStatsScreen(%this)
{
    %time = SomeDudeConcept.endDriveTime - SomeDudeConcept.startDriveTime;
    SomeDudeConcept.statsScreen = new TextSprite()
                                {
                                Scene = SandboxScene;
                                Font = "ToyAssets:TrajanProFont";
                                FontSize = 0.75;
                                Text = "Total Time:" SPC %time SPC "seconds" SPC "Objects Delivered:" SPC SomeDudeConcept.deliveredObjects.count() SPC "Objects Lost:" SPC SomeDudeConcept.lostObjects.count() SPC "";
                                Position = SomeDudeConcept.truckBody.getPositionX() SPC SomeDudeConcept.truckBody.getPositionY()+1;
                                Size = "12 7";
                                OverflowModeY = "visible";
                                TextAlignment = "center";
                                BlendColor = "blue";
                                GravityScale = "0.0";
                                };
}

// -----------------------------------------------------------------------------

function SomeDudeConcept::createTrailer( %this, %posX, %posY )
{
    // Trailer body
    
    SomeDudeConcept.TrailerBody = new Sprite() { class = "TrailerBody"; };
    SomeDudeConcept.TrailerBody.allowManipulationPull = false; // Do not let us pick the truck up
    SomeDudeConcept.TrailerBody.setCollisionCallback(true); // Turn on the collision callback for the truck, we're only going to care about the ones that involve the sensor
    SomeDudeConcept.TrailerBody.setPosition( %posX, %posY );
    SomeDudeConcept.TrailerBody.setImage( "ToyAssets:Blank" );
    SomeDudeConcept.TrailerBody.setBlendAlpha(0);
    SomeDudeConcept.TrailerBody.setImageFrame(0);
    SomeDudeConcept.TrailerBody.setSize( 5.12, 2.347 );
    SomeDudeConcept.TrailerBody.setSceneLayer( SomeDudeConcept.TruckImage );
    SomeDudeConcept.TrailerBody.setSceneGroup( SomeDudeConcept.ColTruckBodyDomain);
    SomeDudeConcept.TrailerBody.setDefaultFriction(SomeDudeConcept.FrictionTruckBody);
    SomeDudeConcept.TrailerBody.setDefaultDensity(SomeDudeConcept.DensityTruckBody);
    SomeDudeConcept.TrailerBody.setDefaultRestitution(SomeDudeConcept.RestitutionTruckBody);
    SomeDudeConcept.TrailerBody.setCollisionGroups( SomeDudeConcept.ColObstacleDomain, SomeDudeConcept.ColGroundDomain, SomeDudeConcept.ColObjectDomain, SomeDudeConcept.ColTruckObjectDomain, SomeDudeConcept.ColTruckObjectDomain+1, SomeDudeConcept.ColTruckObjectDomain-1 );
         
    SomeDudeConcept.TrailerBody.createPolygonCollisionShape(CollisionPTM("-253.0000 -59.0000 -253.0000 -70.0000 40.0000 -70.0000 40.0000 -59.0000")); // Bottom of truck/truck bed
    //SomeDudeConcept.TrailerBody.createPolygonCollisionShape(CollisionPTM("22.0000 50.0000 21.0000 -59.0000 149.0000 -77.0000 138.0000 4.0000 95.0000 48.0000")); // Cab/Door
    //SomeDudeConcept.TrailerBody.createPolygonCollisionShape(CollisionPTM("255.0000 -75.0000 253.0000 -6.0000 138.0000 4.0000 149.0000 -77.0000")); // Hood
    SomeDudeConcept.TrailerBody.createPolygonCollisionShape(CollisionPTM("-241.0000 1.0000 -253.0000 1.0000 -253.0000 -59.0000 -241.0000 -59.0000")); // Tailgate
    SomeDudeConcept.TrailerBody.createPolygonCollisionShape(CollisionPTM("9.0000 1.0000 21.0000 1.0000 21.0000 -59.0000 9.0000 -59.0000")); // Frontgate

    SandboxScene.setDebugOn("joints", "collision");
    
    SandboxScene.add( SomeDudeConcept.TrailerBody );
    Sandbox.setObjectNoManipulation(SomeDudeConcept.TrailerBody, true);

    // Rear tire.   
    %tireRear = new Sprite();
    %tireRear.setPosition( %posX-1.8, %posY-1.3 );
    %tireRear.setImage( "SomeDudeConcept:tires" );
    %tireRear.setSize( 1, 1 );
    %tireRear.setSceneLayer( SomeDudeConcept.Wheel );
    %tireRear.setSceneGroup( SomeDudeConcept.ColWheelDomain );
    %tireRear.setCollisionGroups( SomeDudeConcept.ColObstacleDomain, SomeDudeConcept.ColGroundDomain );

    %tireRear.setDefaultFriction( SomeDudeConcept.WheelFriction );
    %tireRear.setDefaultDensity( SomeDudeConcept.RearWheelDensity );
    %tireRear.setDefaultRestitution(SomeDudeConcept.RearWheelRestitution);
    %tireRear.createCircleCollisionShape(PTM(36.218)); 
    SandboxScene.add( %tireRear );
    SomeDudeConcept.TrailerRearWheel = %tireRear;
    Sandbox.setObjectNoManipulation(%tireRear, true);
   

    // Suspension joints.
    //SomeDudeConcept.RearMotorJoint = SandboxScene.createWheelJoint( SomeDudeConcept.TruckBody, %tireRear, "-1.78 -1.13", "0 0", "0 1" );
    //SomeDudeConcept.FrontMotorJoint = SandboxScene.createWheelJoint( SomeDudeConcept.TruckBody, %tireFront, "2.65 -1.13", "0 0", "0 1" );     
    SomeDudeConcept.TrailerRearMotorJoint = SandboxScene.createWheelJoint( SomeDudeConcept.TrailerBody, %tireRear, "-1.78 -1.20", "0 0", "0 1" );
    //SomeDudeConcept.TrailerFrontMotorJoint = SandboxScene.createWheelJoint( SomeDudeConcept.TrailerBody, %tireFront, "-0.53 -1.20", "0 0", "0 1" );  
    
    // Edit: Set their frequency
    //SandboxScene.setWheelJointFrequency(SomeDudeConcept.TrailerFrontMotorJoint, 6);
    SandboxScene.setWheelJointFrequency(SomeDudeConcept.TrailerRearMotorJoint, 6);

    // Hitch the trailer to the truck body
    SomeDudeConcept.TrailerBody.setAngle(0);
    //SomeDudeConcept.TrailerJoint = SandboxScene.createDistanceJoint(SomeDudeConcept.TrailerBody, SomeDudeConcept.TruckBody, "0.5 -1.0", "-3.4 -1.0");
    //SandboxScene.setDistanceJointFrequency(SomeDudeConcept.TrailerJoint, 0.0);
    //SandboxScene.setDistanceJointLength(SomeDudeConcept.TrailerJoint, 0.25);
    //SandboxScene.setDistanceJointDamping(SomeDudeConcept.TrailerJoint, 0.75);
    SomeDudeConcept.TrailerJoint = SandboxScene.createRevoluteJoint(SomeDudeConcept.TrailerBody, SomeDudeConcept.TruckBody, "0.5 -1.0", "-3.4 -1.0");
    SandboxScene.setRevoluteJointLimit(SomeDudeConcept.TrailerJoint, true, -70.0, 20.0);
    //SandboxScene.setDistanceJointLength(SomeDudeConcept.TrailerJoint, 0.25);
    //SandboxScene.setDistanceJointDamping(SomeDudeConcept.TrailerJoint, 0.75);
}

function SomeDudeConcept::createTruck( %this, %posX, %posY )
{
    // Truck Body.
    %exhaustParticles = new ParticlePlayer();
    %exhaustParticles.setPosition( %posX-3, %posY );
    %exhaustParticles.setSceneLayer( SomeDudeConcept.TruckDecals);
    %exhaustParticles.Particle = "SomeDudeConcept:exhaust";
    %exhaustParticles.SizeScale = 0.1;
    %exhaustParticles.ForceScale = 0.4;
    %exhaustParticles.EmissionRateScale = 4;
    SandboxScene.add( %exhaustParticles );
    %exhaustParticles.play();
    SomeDudeConcept.TruckExhaust = %exhaustParticles;
    Sandbox.setObjectNoManipulation(%exhaustParticles, true);
    SomeDudeConcept.TruckBody = new Sprite() { class = "TruckBody"; };
    SomeDudeConcept.TruckBody.allowManipulationPull = false; // Do not let us pick the truck up
    SomeDudeConcept.TruckBody.setCollisionCallback(true); // Turn on the collision callback for the truck, we're only going to care about the ones that involve the sensor
    SomeDudeConcept.TruckBody.setPosition( %posX, %posY );
    SomeDudeConcept.TruckBody.setImage( "SomeDudeConcept:truckBody" );
    SomeDudeConcept.TruckBody.setImageFrame(0);
    SomeDudeConcept.TruckBody.setSize( 6.827, 2.347 );
    SomeDudeConcept.TruckBody.setSceneLayer( SomeDudeConcept.TruckImage );
    SomeDudeConcept.TruckBody.setSceneGroup( SomeDudeConcept.ColTruckBodyDomain);
    SomeDudeConcept.TruckBody.setDefaultFriction(SomeDudeConcept.FrictionTruckBody);
    SomeDudeConcept.TruckBody.setDefaultDensity(SomeDudeConcept.DensityTruckBody);
    SomeDudeConcept.TruckBody.setDefaultRestitution(SomeDudeConcept.RestitutionTruckBody);
    SomeDudeConcept.TruckBody.setCollisionGroups( SomeDudeConcept.ColObstacleDomain, SomeDudeConcept.ColGroundDomain, SomeDudeConcept.ColObjectDomain, SomeDudeConcept.ColTruckObjectDomain, SomeDudeConcept.ColTruckObjectDomain+1, SomeDudeConcept.ColTruckObjectDomain-1 );
         
    SomeDudeConcept.TruckBody.createPolygonCollisionShape(CollisionPTM("-256.0000 -59.0000 -256.0000 -78.0000 149.0000 -77.0000 21.0000 -59.0000")); // Bottom of truck/truck bed
    SomeDudeConcept.TruckBody.createPolygonCollisionShape(CollisionPTM("22.0000 50.0000 21.0000 -59.0000 149.0000 -77.0000 138.0000 4.0000 95.0000 48.0000")); // Cab/Door
    SomeDudeConcept.TruckBody.createPolygonCollisionShape(CollisionPTM("255.0000 -75.0000 253.0000 -6.0000 138.0000 4.0000 149.0000 -77.0000")); // Hood
    SomeDudeConcept.TruckBody.createPolygonCollisionShape(CollisionPTM("-231.0000 1.0000 -242.0000 1.0000 -243.0000 -59.0000 -231.0000 -59.0000")); // Tailgate

    SomeDudeConcept.TruckBody.createPolygonCollisionShape(CollisionPTM("-222.0000 86.0000 -222.0000 2.0000 -223.0000 2.0000 -223.0000 86.0000")); // Poles in the back of the truck
    //SomeDudeConcept.TruckBody.createPolygonCollisionShape(CollisionPTM("-112.0000 86.0000 -112.0000 2.0000 -113.0000 2.0000 -113.0000 86.0000")); // Uncomment this to add the center pole back
    SomeDudeConcept.TruckBody.createPolygonCollisionShape(CollisionPTM("3.0000 86.0000 3.0000 2.0000 2.0000 2.0000 2.0000 86.0000"));

    //SomeDudeConcept.TruckBody.createPolygonCollisionShape(CollisionPTM("-231.0000 1.0000 -231.0000 -59.0000 21.0000 -59.0000 21.0000 0.0000")); // Sensor shape for the truck bed - old (just to the height of the truck bed)
    SomeDudeConcept.TruckBody.createPolygonCollisionShape(CollisionPTM("-231.0000 81.0000 -231.0000 -59.0000 21.0000 -59.0000 21.0000 81.0000")); // Sensor shape for the truck bed - new (extends to the height of the poles)
    SomeDudeConcept.TruckBody.setCollisionShapeIsSensor(6, true); // This is the 'sensor' in the truck bed. We use it to turn on/off collision with the wheels
    SomeDudeConcept.TruckBody.setCollisionShapeDensity(6, 0); // Change the sensor to have no weight/density
    // Line above was changed from (7, true) to (6, true) because we commented out the center pole for now
    //SomeDudeConcept.TruckBody.setCollisionShapeDensity(1, 4); // Change the sensor to have no weight/density
    
    SandboxScene.add( SomeDudeConcept.TruckBody );
    Sandbox.setObjectNoManipulation(SomeDudeConcept.TruckBody, true);

    // Attach the exhaust output to the truck body.   
    %joint = SandboxScene.createRevoluteJoint( SomeDudeConcept.TruckBody, SomeDudeConcept.TruckExhaust, "-3.3 -1.0", "0 0" );
    SandboxScene.setRevoluteJointLimit( %joint, 0, 0 );

    SandboxWindow.mount( SomeDudeConcept.TruckBody, "3 0.5", 3, true, SomeDudeConcept.RotateCamera ); 

    // Rear tire.   
    %tireRear = new Sprite();
    %tireRear.setPosition( %posX-1.8, %posY-1.3 );
    %tireRear.setImage( "SomeDudeConcept:tires" );
    %tireRear.setSize( 1, 1 );
    %tireRear.setSceneLayer( SomeDudeConcept.Wheel );
    %tireRear.setSceneGroup( SomeDudeConcept.ColWheelDomain );
    %tireRear.setCollisionGroups( SomeDudeConcept.ColObstacleDomain, SomeDudeConcept.ColGroundDomain );

    %tireRear.setDefaultFriction( SomeDudeConcept.WheelFriction );
    %tireRear.setDefaultDensity( SomeDudeConcept.RearWheelDensity );
    %tireRear.setDefaultRestitution(SomeDudeConcept.RearWheelRestitution);
    %tireRear.createCircleCollisionShape(PTM(36.218)); 
    SandboxScene.add( %tireRear );
    SomeDudeConcept.RearWheel = %tireRear;
    Sandbox.setObjectNoManipulation(%tireRear, true);
    
    // Front tire.
    %tireFront = new Sprite();
    %tireFront.setPosition( %posX+2.5, %posY-1.3 );
    %tireFront.setImage( "SomeDudeConcept:tires" );
    %tireFront.setSize( 1, 1 );
    %tireFront.setSceneLayer( SomeDudeConcept.Wheels);
    %tireFront.setSceneGroup( SomeDudeConcept.ColWheelDomain );
    %tireFront.setCollisionGroups( SomeDudeConcept.ColObstacleDomain, SomeDudeConcept.ColGroundDomain );
    
    %tireFront.setDefaultFriction( SomeDudeConcept.WheelFriction );
    %tireFront.setDefaultDensity( SomeDudeConcept.FrontWheelDensity );
    %tireFront.setDefaultRestitution(SomeDudeConcept.FrontWheelRestitution);
    %tireFront.createCircleCollisionShape(PTM(36.218)); 
    SandboxScene.add( %tireFront );   
    SomeDudeConcept.FrontWheel = %tireFront;
    Sandbox.setObjectNoManipulation(%tireFront, true);

    // Suspension joints.
    //SomeDudeConcept.RearMotorJoint = SandboxScene.createWheelJoint( SomeDudeConcept.TruckBody, %tireRear, "-1.78 -1.13", "0 0", "0 1" );
    //SomeDudeConcept.FrontMotorJoint = SandboxScene.createWheelJoint( SomeDudeConcept.TruckBody, %tireFront, "2.65 -1.13", "0 0", "0 1" );     
    SomeDudeConcept.RearMotorJoint = SandboxScene.createWheelJoint( SomeDudeConcept.TruckBody, %tireRear, "-1.78 -1.20", "0 0", "0 1" );
    SomeDudeConcept.FrontMotorJoint = SandboxScene.createWheelJoint( SomeDudeConcept.TruckBody, %tireFront, "2.65 -1.20", "0 0", "0 1" );  
    
    // Debug - Try using rope joints to maintain a set distance betwen the wheel and the truck body
    SomeDudeConcept.RearWheelRope1 = SandboxScene.createRopeJoint(SomeDudeConcept.TruckBody, %tireRear, "-1.78 -1.10", "0 0", 0.3);
    SomeDudeConcept.RearWheelRope2 = SandboxScene.createRopeJoint(SomeDudeConcept.TruckBody, %tireRear, "-1.78 -1.30", "0 0", 0.3);
    SomeDudeConcept.FrontWheelRope1 = SandboxScene.createRopeJoint(SomeDudeConcept.TruckBody, %tireFront, "2.65 -1.10", "0 0", 0.3);
    SomeDudeConcept.FrontWheelRope2 = SandboxScene.createRopeJoint(SomeDudeConcept.TruckBody, %tireFront, "2.65 -1.30", "0 0", 0.3);

    // Edit: Set their frequency
    SandboxScene.setWheelJointFrequency(SomeDudeConcept.FrontMotorJoint, 6);
    SandboxScene.setWheelJointFrequency(SomeDudeConcept.RearMotorJoint, 6);

    // Debug - add the nightMask for testing
    //SomeDudeConcept.nightMask = new Sprite();
    //SomeDudeConcept.nightMask.setImage("SomeDudeConcept:nightMask");
    //SomeDudeConcept.nightMask.setSize(27.3, 13.65);
    //SomeDudeConcept.nightMask.setPosition(SomeDudeConcept.truckBody.getPosition());
    //SomeDudeConcept.nightMask.setFixedAngle(false);
    //Sandbox.setObjectNoManipulation(SomeDudeConcept.nightMask, true);
    //SomeDudeConcept.nightMask.setGravityScale(0.0);
    //SandboxScene.add(SomeDudeConcept.nightMask);
    //SandboxScene.createWeldJoint(SomeDudeConcept.nightMask, SomeDudeConcept.truckBody, "0 0", "-3.41 0", 0, 1);
}

// -----------------------------------------------------------------------------

function SomeDudeConcept::createDriver(%this)
{ // Read our driver parts in from the TAML file, add to the scene and assemble to the truck body
    %simset = tamlread("dude.taml");
    %count = %simset.getCount();
    %index = 0;
    while (%index < %count)
        {
        SandboxScene.add(%simset.getObject(%index)); // Add the object to the scene
        //%simset.getObject(%index).setBodyType("kinematic");
        switch(%index)
            {
            case 0 : somedudeconcept.torso = %simset.getObject(%index);
            case 1 : somedudeconcept.head = %simset.getObject(%index);
            case 2 : somedudeconcept.r_low = %simset.getObject(%index);
            case 3 : somedudeconcept.r_up = %simset.getObject(%index);
            case 4 : somedudeconcept.l_low = %simset.getObject(%index);
            case 5 : somedudeconcept.l_up = %simset.getObject(%index);
            }
        %index++;
        }
    // All objects have been added to the scene. Set up the joints
    //SomeDudeConcept.jointHead2 = Sandboxscene.createropejoint(somedudeconcept.head, somedudeconcept.truckbody, "0 0", "0.91 0.5", 0.2);
    SomeDudeConcept.jointHead1 = Sandboxscene.createropejoint(somedudeconcept.torso, somedudeconcept.head, "0 0", "-0.01 -0.325", 0.02);
    SomeDudeConcept.jointTorso = SandboxScene.createropejoint(SomeDudeConcept.torso, SomeDudeConcept.truckbody, "0 0", "0.91 0.0369", 0.05);
    SomeDudeConcept.jointWheel1 = SandboxScene.createropejoint(SomeDudeConcept.r_low, SomeDudeConcept.truckbody, "0 0", "1.1 -0.0131", 0.05);
    SomeDudeConcept.jointWheel2 = SandboxScene.createropejoint(SomeDudeConcept.l_low, SomeDudeConcept.truckbody, "0 0", "1.15 0.0269", 0.05);
    SomeDudeConcept.jointR1 = SandboxScene.createropejoint(SomeDudeConcept.r_low, SomeDudeConcept.r_up, "0 0", "0.27 -0.04", 0.05);
    SomeDudeConcept.jointR2 = SandboxScene.createropejoint(SomeDudeConcept.r_up, SomeDudeConcept.torso, "-0.03 -0.01", "0 0", 0.05);
    SomeDudeConcept.jointL1 = SandboxScene.createropejoint(SomeDudeConcept.l_low, SomeDudeConcept.l_up, "0 0", "0.17 -0.03", 0.05);
    SomeDudeConcept.jointL2 = SandboxScene.createropejoint(SomeDudeConcept.l_up, SomeDudeConcept.torso, "0.09 0.02", "0 0", 0.05);

    SomeDudeConcept.Torso.createCircleCollisionShape(PTM(10.0)); 
    SomeDudeConcept.Torso.setCollisionShapeDensity(0, 5); // Change the sensor to have no weight/density
}

function truckForward(%val, %percent)
{
    if(%val)
    { 
        if ( !SomeDudeConcept.TruckMoving )
        {
            %driveActive = false;
            SomeDudeConcept.TruckBody.setImageFrame(1); // Make sure we're braked
            if ( SomeDudeConcept.FrontWheelDrive )
            {
                SandboxScene.setWheelJointMotor( SomeDudeConcept.FrontMotorJoint, true, -SomeDudeConcept.WheelSpeed * %percent, 10000 );
                %driveActive = true;
            }
            else
            {
                SandboxScene.setWheelJointMotor( SomeDudeConcept.FrontMotorJoint, false );
            }
            
            if ( SomeDudeConcept.RearWheelDrive )
            {
                SandboxScene.setWheelJointMotor( SomeDudeConcept.RearMotorJoint, true, -SomeDudeConcept.WheelSpeed * %percent, 10000 );
                %driveActive = true;
            }
            else
            {
                SandboxScene.setWheelJointMotor( SomeDudeConcept.RearMotorJoint, false );
            }            
            
            if ( %driveActive )
            {
                SomeDudeConcept.TruckExhaust.SizeScale *= 4;
                SomeDudeConcept.TruckExhaust.ForceScale /= 2;
            }
        }
              
        SomeDudeConcept.TruckMoving = true;
    }
    else
    {
        SomeDudeConcept.truckStop();
    }
}

// -----------------------------------------------------------------------------

function truckReverse(%val, %percent)
{
    if(%val)
    {
        if ( !SomeDudeConcept.TruckMoving )
        {
            %driveActive = false;
            if ( SomeDudeConcept.FrontWheelDrive )
            {
                SandboxScene.setWheelJointMotor( SomeDudeConcept.FrontMotorJoint, true, SomeDudeConcept.WheelSpeed * %percent, 10000 );
                %driveActive = true;
            }
            else
            {
                SandboxScene.setWheelJointMotor( SomeDudeConcept.FrontMotorJoint, false );
            }
            if ( SomeDudeConcept.RearWheelDrive )
            {
                SandboxScene.setWheelJointMotor( SomeDudeConcept.RearMotorJoint, true, SomeDudeConcept.WheelSpeed * %percent, 10000 );
                %driveActive = true;
            }
            else
            {
                SandboxScene.setWheelJointMotor( SomeDudeConcept.RearMotorJoint, false );
            }
            
            if ( %driveActive )
            {
                SomeDudeConcept.TruckExhaust.SizeScale *= 4;
                SomeDudeConcept.TruckExhaust.ForceScale /= 2;
            }            
        }
              
        SomeDudeConcept.TruckMoving = true;
    }
    else
    {
        SomeDudeConcept.truckStop();
    }
}

//-----------------------------------------------------------------------------

function SomeDudeConcept::truckStop(%this)
{
    // Finish if truck is not moving.
    if ( !SomeDudeConcept.TruckMoving )
        return;

    // Stop truck moving.
    //SandboxScene.setWheelJointMotor( SomeDudeConcept.RearMotorJoint, true, SomeDudeConcept.WheelSpeed/2, 10000 ); // Edited for testing
    //SandboxScene.setWheelJointMotor( SomeDudeConcept.FrontMotorJoint, true, SomeDudeConcept.WheelSpeed/2, 10000 );
    SandboxScene.setWheelJointMotor( SomeDudeConcept.RearMotorJoint, true, 0, 10000 );
    SandboxScene.setWheelJointMotor( SomeDudeConcept.FrontMotorJoint, true, 0, 10000 );
    SomeDudeConcept.TruckExhaust.SizeScale /= 4;
    SomeDudeConcept.TruckExhaust.ForceScale *= 2;

    // Flag truck as not moving.    
    SomeDudeConcept.TruckMoving = false;
}

//-----------------------------------------------------------------------------
function SomeDudeConcept::setTilePositionX (%this, %value )
{
    %this.TilePositionX = %value;
}

function SomeDudeConcept::setTilePositionY (%this, %value )
{
    %this.TilePositionY = %value;
}

function SomeDudeConcept::setTileCountX (%this, %value )
{
    %this.TileCountX = %value;
}

function SomeDudeConcept::setTileCountY (%this, %value )
{
    %this.TileCountY = %value;
}

function SomeDudeConcept::setTileSizeX (%this, %value )
{
    %this.TileSizeX = %value;
}

function SomeDudeConcept::setTileSizeY (%this, %value )
{
    %this.TileSizeY = %value;
}

function SomeDudeConcept::setTileSetName (%this, %value )
{
    %this.TileSetName = %value;
}

function SomeDudeConcept::setTileSetCount (%this, %value )
{
    %this.TileSetCount = %value;
}
//-----------------------------------------------------------------------------

function SomeDudeConcept::onTouchDragged(%this, %touchID, %worldPosition)
{
    // Do nothing if the mouse isn't held down
    if (SomeDudeConcept.touchDown == false)
        return;
    // Do nothing if we are in manipulation mode
    if (SomeDudeConcept.allowTruckDriving == false)
        return;
    // If we touch in-front of the truck then move forward else reverse.
    if (%worldPosition.x < SomeDudeConcept.TruckBody.Position.x)
        { // Reverse
        %difference = SomeDudeConcept.TruckBody.Position.x - %worldPosition.x;
        if (%difference >= 5)
            %val = 1;
        else
            %val = %difference / 5;
        SomeDudeConcept.truckStop();
        truckReverse(true, %val);
        SomeDudeConcept.TruckBody.setImageFrame(1); // Make sure we're going reverse/braked
        }

    if (%worldPosition.x >= SomeDudeConcept.TruckBody.Position.x)
        { // Forward
        %difference = %worldPosition.x - SomeDudeConcept.TruckBody.Position.x;
        if (%difference >= 5)
            %val = 1;
        else
            %val = %difference / 5;
        SomeDudeConcept.truckStop();
        truckForward(true, %val);
        SomeDudeConcept.TruckBody.setImageFrame(0); // Make sure we're going forward
        }
}

function SomeDudeConcept::onTouchDown(%this, %touchID, %worldPosition)
{
    if (SomeDudeConcept.StartUp $= "ground_editor")
        { // Ground Collision editor
        SomeDudeConcept.addGroundPoint(%worldPosition);
        //echo("GroundCollisionLeftDown:" SPC %worldPosition);
        return; // Exit out
        }    
    SomeDudeConcept.touchDown = true;
    // Do nothing if we are in manipulation mode
    if (SomeDudeConcept.allowTruckDriving == false)
        return;
    
    // Finish if truck is already moving.
    if ( SomeDudeConcept.TruckMoving )
        return;

    
    // If we touch in-front of the truck then move forward else reverse.
    if (%worldPosition.x < SomeDudeConcept.TruckBody.Position.x)
        { // Reverse
        %difference = SomeDudeConcept.TruckBody.Position.x - %worldPosition.x;
        if (%difference >= 5)
            %val = 1;
        else
            %val = %difference / 5;
        truckReverse(true, %val);
        SomeDudeConcept.TruckBody.setImageFrame(1); // Make sure we're going reverse/braking
        }

    if (%worldPosition.x >= SomeDudeConcept.TruckBody.Position.x)
        { // Forward
        %difference = %worldPosition.x - SomeDudeConcept.TruckBody.Position.x;
        if (%difference >= 5)
            %val = 1;
        else
            %val = %difference / 5;
        truckForward(true, %val);
        SomeDudeConcept.TruckBody.setImageFrame(0); // Make sure we're going forward
        }
}

//-----------------------------------------------------------------------------

function SomeDudeConcept::onTouchUp(%this, %touchID, %worldPosition)
{
    SomeDudeConcept.touchDown = false;
    // Do nothing if we are in manipulation mode
   if (SomeDudeConcept.allowTruckDriving == false)
        return;

    // Stop the truck.
    SomeDudeConcept.truckStop();
    SomeDudeConcept.TruckBody.setImageFrame(1); // Make sure we're braked
}

function Grandpa::onUpdate(%this)
{
    // Testing on update to rotate Grandpa to 0 degrees on a regular basis
    %this.rotateTo(0.0, 250.0, true, true);
}

function SomeDudeConcept::panLevel(%this, %array, %speed)
{ // Mounts the camera to an invisible sprite that moves across the level to pan all of the objects dropped by the player
%camera = new Sprite();
%camera.setImage("ToyAssets::blank");
%camera.setBlendAlpha(0.0);
%camera.setGravityScale(0);
%object = %array.getKey(0); // set initial object to the first object in the array
%camera.setPosition(%object.getPosition());
SomeDudeConcept.camera = %camera;
SandboxScene.add(%camera); // Add to the scene
SandboxWindow.mount( SomeDudeConcept.camera, "0 0", 3, true, false); 
%camera.moveTo(%array.getKey(1).getPosition(), %speed, true);
}
