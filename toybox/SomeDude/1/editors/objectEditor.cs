//-----------------------------------------------------------------------------
// objectEditor
// A simple tool for creating and moving objects
//-----------------------------------------------------------------------------

// Object Manipulation
//---------------------------------------------
function flipSpriteX()
{
%pickedObject = Sandbox.ManipulationPullObject[SomeDudeConcept.currentObject];
if (!isObject(%pickedObject))
    return; // Skip out if we have no object being manipulated
%flipped = %pickedObject.getFlipX();
if (%flipped)
    %pickedObject.setFlipX(false);
else
    %pickedObject.setFlipX(true);
}

function flipSpriteY()
{
%pickedObject = Sandbox.ManipulationPullObject[SomeDudeConcept.currentObject];
if (!isObject(%pickedObject))
    return; // Skip out if we have no object being manipulated
%flipped = %pickedObject.getFlipY();
if (%flipped)
    %pickedObject.setFlipY(false);
else
    %pickedObject.setFlipY(true);
}

function rotateSpriteL()
{ // Rotate a sprite counter-clockwise by 45 degrees
%pickedObject = Sandbox.ManipulationPullObject[SomeDudeConcept.currentObject];
if (!isObject(%pickedObject))
    return; // Skip out if we have no object being manipulated
%rotation = %pickedObject.getAngle();
%rotation = %rotation - 45.0;
if (%rotation < 0)
    %rotation = %rotation+360;
%pickedObject.setAngle(%rotation);
}

function rotateSpriteR()
{ // Rotate a sprite clockwise by 45 degrees
%pickedObject = Sandbox.ManipulationPullObject[SomeDudeConcept.currentObject];
if (!isObject(%pickedObject))
    return; // Skip out if we have no object being manipulated
%rotation = %pickedObject.getAngle();
%rotation = %rotation + 45.0;
if (%rotation > 360)
    %rotation = %rotation-360;
%pickedObject.setAngle(%rotation);
}

// World Unit Conversation
//-------------------------------------------------------------------------------
function PTM(%pixels)
{ // Converts between pixels and meters (used to convert PhysicalEditor collision shapes to Box2D units)
    %meters = %pixels * 0.013333984375; // Based off 512 pixels per 6.827 meters
    return %meters;
}

function CollisionPTM(%coords)
{ // A wrapper for converting a string of coordinates from pixel units to meters
    %total = getWordCount(%coords); // Get how many separate words we have
    %new_coords = ""; // This will be our new string
    %count = 0;
    while (%count < %total)
        {
        %new_coords = %new_coords SPC PTM(getWord(%coords, %count));
        %count++;
        }
    return %new_coords;
}

// Object creation/setup/spawning functions
//------------------------------------------------------------------------------
function SomeDudeConcept::moveObjectsToArray(%this, %simset, %array, %addToScene)
{ // Goes through a simset and moves all objects present in the simset to a specified arrayObject
    // if Add to Scene is true, then we also add the object to the sandbox scene
    %count = %simset.getCount();
    %index = 0;
    while (%index < %count)
    {
    %array.push_Back(%simset.getObject(%index), 0);
    if (%addToScene)
        SandboxScene.add(%simset.getObject(%index));
    // Check for any composite objects that need a joint created
    %object = %simset.getObject(%index);
    if (%object.class $= "lamp")
        {
        echo("Found lamp");
        if (%object.setUp != true)
            {
            %object.setUp = true;
            %shade = %object.other;
            SomeDudeConcept.LampJoint = SandboxScene.createRevoluteJoint( %object, %shade, CollisionPTM("0.3 10.1"), CollisionPTM("0.3 10.1"), false);
            SandboxScene.setRevoluteJointLimit(SomeDudeConcept.LampJoint, true, -10, 10); // set up the joint
            }
        }
    %index++;
    }
}

function SomeDudeConcept::SpawnObject(%this, %posX, %posY, %type)
{            
   if (%type $= "grandpa")
        {
        SomeDudeConcept.setUpGrandpa(%posX, %posY);
        return;
        }
    else if (%type $= "lamp1")
        {
        SomeDudeConcept.setUpLamp1(%posX, %posY);     
        return;
        }
    else if (%type $= "barstool")
        {
        %object = new Sprite();
        %object.setBullet(true);
        %object.class = "chair";
        %object.setImage("SomeDudeConcept:barStool");
        %object.setSize("0.75 1.0");
        %object.objectType = "household";
        %object.setDefaultFriction(SomeDudeConcept.FrictionFurniture);
        %object.setDefaultDensity(SomeDudeConcept.DensityFurniture);
        %object.setDefaultRestitution(SomeDudeConcept.RestitutionFurniture);
        %object.createPolygonCollisionShape(CollisionPTM("-22.0 35.5 -22.0 29.5 21.0 29.5 21.0 35.5"));
        %object.createPolygonCollisionShape(CollisionPTM("17.0 -37.5 17.0 29.5 11.0 -32.5 11.0 -37.5"));
        %object.createPolygonCollisionShape(CollisionPTM("-18.0 29.5 -18.0 -32.5 11.0 -32.5 17.0 29.5"));
        %object.createPolygonCollisionShape(CollisionPTM("-18.0 -32.5 -18.0 -37.5 -12.0 -37.5 -12.0 -32.5"));
        SomeDudeConcept.createdObjects.push_Back(%object, 0); // Add our object to the array
        }
    else if (%type $= "crate1")
        {
        %object = new Sprite();
        %object.setBullet(true);
        %size = 0.75;
        %object.class = "box";
        %object.setImage("ToyAssets:Crate");     
        %object.setSize(%size);
        %object.objectType = "household";
        %object.setDefaultFriction(SomeDudeConcept.FrictionBoxes);
        %object.setDefaultDensity(SomeDudeConcept.DensityBoxes);
        %object.setDefaultRestitution(SomeDudeConcept.RestitutionBoxes);
        %object.createPolygonBoxCollisionShape(%size, %size);
        SomeDudeConcept.createdObjects.push_Back(%object, 0); // Add our object to the array
        }
    else if (%type $= "crate2")
        {
        %object = new Sprite();
        %object.setBullet(true);
        %size = 1;
        %object.class = "box";
        %object.setImage("ToyAssets:Tiles");     
        %object.setImageFrame(2);
        %object.setSize(%size);
        %object.objectType = "household";
        %object.setDefaultFriction(SomeDudeConcept.FrictionBoxes);
        %object.setDefaultDensity(SomeDudeConcept.DensityBoxes);
        %object.setDefaultRestitution(SomeDudeConcept.RestitutionBoxes);
        %object.createPolygonBoxCollisionShape(%size, %size);
        SomeDudeConcept.createdObjects.push_Back(%object, 0); // Add our object to the array
        }
    else if (%type $= "ballSoccer")
        {
        SomeDudeConcept.createBall(%posX, %posY);
        return;
        }
    else if (%type $= "chairDining1")
        {
        %object = new Sprite();
        %object.setBullet(true);
        %object.class = "chair";
        %object.size = 1.0 SPC 1.5;
        %object.setImage("SomeDudeConcept:chairDining1");
        %object.objectType = "household";
        %object.setDefaultFriction(SomeDudeConcept.FrictionFurniture);
        %object.setDefaultDensity(SomeDudeConcept.DensityFurniture);
        %object.setDefaultRestitution(SomeDudeConcept.RestitutionFurniture);
        %object.createPolygonCollisionShape(CollisionPTM("32.5 -10.0 -21.5 -10.0 -20.5 -24.0 32.5 -24.0"));
        %object.createPolygonCollisionShape(CollisionPTM("32.5 -56.0 32.5 -24.0 24.5 -24.0 24.5 -56.0"));
        %object.createPolygonCollisionShape(CollisionPTM("-20.5 -56.0 -20.5 -24.0 -21.5 -10.0 -28.5 -19.0 -28.5 -56.0"));
        %object.createPolygonCollisionShape(CollisionPTM("-21.5 54.0 -33.5 54.0 -33.5 -19.0 -28.5 -19.0 -21.5 -10.0"));
        SomeDudeConcept.createdObjects.push_Back(%object, 0); // Add our object to the array
        }
    else if (%type $= "chairDining2")
        {
        %object = new Sprite();
        %object.setBullet(true);
        %object.class = "chair";
        %object.size = 1.0 SPC 1.5;
        %object.setImage("SomeDudeConcept:chairDining2");
        %object.setDefaultFriction(SomeDudeConcept.FrictionFurniture);
        %object.setDefaultDensity(SomeDudeConcept.DensityFurniture);
        %object.setDefaultRestitution(SomeDudeConcept.RestitutionFurniture);
        %object.createPolygonCollisionShape(CollisionPTM("-27.5 55.0 -27.5 -10.0 -23.5 -24.0 21.5 -24.0 25.5 -10.0 25.5 55.0"));
        %object.createPolygonCollisionShape(CollisionPTM("28.5 -56.0 28.5 -17.0 25.5 -10.0 21.5 -24.0 21.5 -56.0"));
        %object.createPolygonCollisionShape(CollisionPTM("-27.5 -10.0 -30.5 -17.0 -30.5 -56.0 -23.5 -56.0 -23.5 -24.0"));
        SomeDudeConcept.createdObjects.push_Back(%object, 0); // Add our object to the array
        }
    else if (%type $= "endTable")
        {
        %object = new Sprite();
        %object.setBullet(true);
        %object.class = "endTable";
        %object.size = 1;
        %object.setImage("SomeDudeConcept:endTable");
        %object.objectType = "household";
        %object.setDefaultFriction(SomeDudeConcept.FrictionFurniture);
        %object.setDefaultDensity(SomeDudeConcept.DensityFurniture);
        %object.setDefaultRestitution(SomeDudeConcept.RestitutionFurniture);
        %object.createPolygonCollisionShape(CollisionPTM("-32.5 37.5 -32.5 -37.5 -22.5 -37.5 -22.5 37.5"));
        %object.createPolygonCollisionShape(CollisionPTM("21.5 37.5 21.5 -37.5 32.5 -37.5 32.5 37.5"));
        %object.createPolygonCollisionShape(CollisionPTM("-22.5 37.5 -22.5 0.5 21.5 0.5 21.5 37.5"));
        SomeDudeConcept.createdObjects.push_Back(%object, 0); // Add our object to the array
        }
    else if (%type $= "throwPillow1")
        {
        %object = new Sprite();
        %object.setBullet(true);
        %object.class = "nonbreakable";
        %object.setSize(0.5, 0.5);
        %object.setImage("SomeDudeConcept:throwPillow1");
        %object.objectType = "household";
        %object.setDefaultDensity(SomeDudeConcept.DensityNonbreakable);
        %object.setDefaultFriction(SomeDudeConcept.FrictionNonbreakable);
        %object.setDefaultRestitution(SomeDudeConcept.RestitutionNonbreakable);
        %object.createPolygonCollisionShape(CollisionPTM("4.0 19.0 -4.0 18.8 -8.0 15.0 -8.0 -15.0 -4.0 -19.0 4.0 -19.0 8.0 -15.0 8.0 15.0"));
        SomeDudeConcept.createdObjects.push_Back(%object, 0); // Add our object to the array
        }
    else if (%type $= "throwPillow2")
        {
        %object = new Sprite();
        %object.setBullet(true);
        %object.class = "nonbreakable";
        %object.setSize(0.5, 0.5);
        %object.setImage("SomeDudeConcept:throwPillow2");
        %object.objectType = "household";
        %object.setDefaultDensity(SomeDudeConcept.DensityNonbreakable);
        %object.setDefaultFriction(SomeDudeConcept.FrictionNonbreakable);
        %object.setDefaultRestitution(SomeDudeConcept.RestitutionNonbreakable);
        %object.createPolygonCollisionShape(CollisionPTM("17.0 18.0 -1.0 16.0 -15.3 0.4 -16.0 -16.0 0.0 -15.0 16.0 -1.0"));
        %object.createPolygonCollisionShape(CollisionPTM("-17.0 18.0 -15.3 0.4 -1.0 16.0"));
        %object.createPolygonCollisionShape(CollisionPTM("17.0 -16.0 16.0 -1.0 0.0 -15.0"));
        SomeDudeConcept.createdObjects.push_Back(%object, 0); // Add our object to the array
        }
    else if (%type $= "cardboardBox")
        {
        %object = new Sprite();
        %object.setBullet(true);
        %object.class = "box";
        %object.setSize(0.75, 0.5);
        %object.setImage("SomeDudeConcept:cardboardBox");
        %object.objectType = "household";
        %object.setDefaultFriction(SomeDudeConcept.FrictionBoxes);
        %object.setDefaultDensity(SomeDudeConcept.DensityBoxes);
        %object.createPolygonCollisionShape(CollisionPTM("-27.0 15.5 -27.0 -19.5 25.0 -19.5 25.0 14.5 1.0 17.5"));
        SomeDudeConcept.createdObjects.push_Back(%object, 0); // Add our object to the array
        }
    else
        {
        echo("Invalid spawn object type" SPC %type);
        return; // skip out if no match found
        }

    %object.setSceneLayer(SomeDudeConcept.ObjectDomain);
    %object.setSceneGroup(SomeDudeConcept.ColObjectDomain);
    %object.setCollisionGroups( SomeDudeConcept.ColObstacleDomain, SomeDudeConcept.ColGroundDomain, SomeDudeConcept.ColObjectDomain, SomeDudeConcept.ColWheelDomain );
   
    %object.setPosition(%posX, %posY);  
    SandboxScene.add(%object);
}

// Added this function for testing - eventually we need to wrap our own stuff up for item creation
function SomeDudeConcept::createBall(%this, %posX, %posY)
{      // Lane is deprecated
    %ball = new Sprite()
        {
            class = "nonBreakable";
        };
    %ball.objectType = "household";
    %ball.currentRow = 0; // default to the mid row
    %ball.SetPosition( %posX, %posY);
    %ball.Size = 0.5;
    %ball.Image = "ToyAssets:Football";        
    %ball.setBullet(true);
    %ball.setDefaultFriction(SomeDudeConcept.FrictionBall);
    %ball.setDefaultDensity(SomeDudeConcept.DensityBall);
    %ball.setDefaultRestitution(SomeDudeConcept.RestitutionBall);        
    %ball.setSceneLayer( SomeDudeConcept.ObjectDomain); // This layer will put them behind the truck image itself, which is what we want asthetically
    %ball.setSceneGroup( SomeDudeConcept.ColObjectDomain);
    %ball.setCollisionGroups( SomeDudeConcept.ColObstacleDomain, SomeDudeConcept.ColGroundDomain, SomeDudeConcept.ColObjectDomain, SomeDudeConcept.ColWheelDomain );
    
    %ball.createCircleCollisionShape(0.25); 
    // Add to the scene.
    SandboxScene.add( %ball );
    SomeDudeConcept.createdObjects.push_Back(%ball, 0); // Add our object to the array
}

function SomeDudeConcept::setUpLamp1(%this, %posX, %posY)
{
    %lamp = new Sprite() { class = "lamp";};
    %lamp.setImage("SomeDudeConcept:lamp1");
    %lamp.setSize("0.5 0.75");
    %lamp.objectType = "household";
    %lamp.compositeObject = true;
    %lamp.setDefaultFriction(SomeDudeConcept.FrictionFurniture);
    %lamp.setDefaultDensity(SomeDudeConcept.DensityFurniture);
    %lamp.setDefaultRestitution(SomeDudeConcept.RestitutionFurniture);
    %lamp.createCircleCollisionShape(CollisionPTM("10.0"), CollisionPTM("0.4 -16.4"));
    %lamp.createPolygonCollisionShape(CollisionPTM("-1.0 25.0 -1.0 -7.0 2.0 -7.0 2.0 25.0"));
    %lamp.createPolygonCollisionShape(CollisionPTM("-10.0 -25.0 -10.0 -28.0 11.0 -28.0 11.0 -25.0"));
    %lamp.setPosition(%posX, %posY);
    %lamp.currentRow = 0;

    %lamp.setSceneLayer( SomeDudeConcept.ObjectDomain);
    %lamp.setSceneGroup( SomeDudeConcept.ColObjectDomain);
    %lamp.setCollisionGroups( SomeDudeConcept.ColObstacleDomain, SomeDudeConcept.ColGroundDomain, SomeDudeConcept.ColObjectDomain, SomeDudeConcept.ColWheelDomain );

    SandboxScene.add(%lamp);
    SomeDudeConcept.createdObjects.push_Back(%lamp, 0); // Add our object to the array
    
    // Add the shade to the lamp
    %shade = new Sprite() { class = "lamp"; };
    %shade.setPosition( %posX, %posY );
    %shade.setImage( "SomeDudeConcept:shade1" );
    %shade.objectType = "household";
    %shade.compositeObject = true;
    %shade.setSize( 0.5, 0.75 );
    %shade.setDefaultFriction(SomeDudeConcept.FrictionNonbreakable);
    %shade.setDefaultDensity(SomeDudeConcept.DensityNonbreakable);
    %shade.setDefaultRestitution(SomeDudeConcept.RestitutionNonbreakable);

    %shade.setSceneLayer( SomeDudeConcept.ObjectDomain);
    %shade.setSceneGroup( SomeDudeConcept.ColObjectDomain);
    %shade.setCollisionGroups( SomeDudeConcept.ColObstacleDomain, SomeDudeConcept.ColGroundDomain, SomeDudeConcept.ColObjectDomain, SomeDudeConcept.ColWheelDomain );

    %shade.createPolygonCollisionShape(CollisionPTM("-11.0 23.0 -11.0 -3.0 12.0 -3.0 12.0 23.0"));

    %shade.currentRow = 0;
    SandboxScene.add( %shade );
    %shade.other = %lamp;
    %lamp.other = %shade; // Make sure these two objects can reference eachother

    SomeDudeConcept.LampJoint= SandboxScene.createRevoluteJoint( %lamp, %shade, CollisionPTM("0.3 10.1"), CollisionPTM("0.3 10.1"), false);
    SandboxScene.setRevoluteJointLimit(SomeDudeConcept.LampJoint, true, -10, 10);
}

function SomeDudeConcept::SetUpGrandpa(%this, %posX, %posY)
{
    %grandpa = new Sprite() { class = "grandpa"; FixedAngle = false; };
    %grandpa.setImage( "SomeDudeConcept:grandpa" );
    %grandpa.setSize( 1.16, 1.5 );
    %grandpa.setPosition( %posX, %posY );
    %grandpa.compositeObject = true;
    %grandpa.currentRow = 0;
    
    %grandpa.setSceneLayer( SomeDudeConcept.ObjectDomain);
    %grandpa.setSceneGroup( SomeDudeConcept.ColObjectDomain);
    %grandpa.setCollisionGroups( SomeDudeConcept.ColObstacleDomain, SomeDudeConcept.ColGroundDomain, SomeDudeConcept.ColObjectDomain, SomeDudeConcept.ColWheelDomain );
   
    %grandpa.setDefaultFriction( SomeDudeConcept.WheelFriction );

    %grandpa.createPolygonCollisionShape(CollisionPTM("8.9840 -26.0000 19.9840 -20.0000 13.2061 -17.4334 5.9840 -15.0000"));
    %grandpa.createPolygonCollisionShape(CollisionPTM("14.9840 15.0000 10.9840 26.0000 3.9840 27.0000 -0.5765 19.1669 6.9840 11.0000"));
    %grandpa.createPolygonCollisionShape(CollisionPTM("-10.0160 -15.0000 5.9840 -15.0000 2.9840 -4.0000 -11.0160 14.0000"));
    %grandpa.createPolygonCollisionShape(CollisionPTM("6.9840 11.0000 -0.5765 19.1669 -11.0160 14.0000 2.9840 6.0000"));
    %grandpa.createPolygonCollisionShape(CollisionPTM("-20.0160 14.0000 -11.0160 14.0000 -0.5765 19.1669 -20.0160 18.0000"));
    %grandpa.createPolygonCollisionShape(CollisionPTM("9.9840 -4.0000 2.9840 -4.0000 5.9840 -15.0000 13.2061 -17.4334"));
    %grandpa.createPolygonCollisionShape(CollisionPTM("2.9840 6.0000 -11.0160 14.0000 2.9840 -4.0000"));
    SandboxScene.add(%grandpa);
    SomeDudeConcept.createdObjects.push_Back(%grandpa, 0); // Add our object to the array
    
    // Add the wheel to his chair
    %wheel = new Sprite() { class = "grandpa"; };
    %wheel.setPosition( %posX-0.2, %posY-0.5 );
    %wheel.setImage( "SomeDudeConcept:grandpaWheel" );
    %wheel.setSize( 0.75, 0.75 );
    %wheel.compositeObject = true;
    %wheel.setSceneLayer( SomeDudeConcept.ObjectDomain);
    %wheel.setSceneGroup( SomeDudeConcept.ColObjectDomain);
    %wheel.setCollisionGroups( SomeDudeConcept.ColObstacleDomain, SomeDudeConcept.ColGroundDomain, SomeDudeConcept.ColObjectDomain, SomeDudeConcept.ColWheelDomain );
    
    %wheel.setDefaultFriction( SomeDudeConcept.WheelFriction+2 );
    %wheel.setDefaultDensity( SomeDudeConcept.RearWheelDensity );
    %wheel.createCircleCollisionShape(CollisionPTM(28.0)); 
    %wheel.currentRow = 0;
    
    SandboxScene.add( %wheel );
    %wheel.other = %grandpa;
    %grandpa.other = %wheel; // Make sure these two objects can reference eachother
    SomeDudeConcept.GrandpaJoint= SandboxScene.createWheelJoint( %grandpa, %wheel, "-0.2 -0.5", "0 0", "0 1" );
    SandboxScene.setWheelJointDampingRatio(SomeDudeConcept.GrandpaJoint, 0.5);
    SandboxScene.setWheelJointFrequency(SomeDudeConcept.GrandpaJoint, 25);  
    %grandpa.setUpdateCallback(true); // turn on his update callback
}