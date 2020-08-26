//-----------------------------------------------------------------------------
// groundEditor
// A simple tool for drawing a ground collision line (basically a chain of
// edge collision shapes)
//-----------------------------------------------------------------------------

function SomeDudeConcept::initializeRandomTerrain(%this)
    { // Initializes random terrain generation
    %array = new array();
    SomeDudeConcept.Ground = %array;
    %groundPlane = new Scroller();
    %groundPlane.setBodyType("static");
    %groundPlane.setImage("ToyAssets:blank"); // Make it an invisible image
    %groundPlane.setBlendAlpha(0.0);
    %groundPlane.setSize(SomeDudeConcept.WorldWidth, 3); // Give it a size, position, etc. and set its layers
    %groundPlane.setPosition(0, (SomeDudeConcept.FloorLevel + 2.75) - (%groundPlane.getSizeY() / 2));
    %groundPlane.setSceneLayer(SomeDudeConcept.BackgroundDomain); // Need to update this
    %groundPlane.setSceneGroup(SomeDudeConcept.ColGroundDomain);
    %groundPlane.setRepeatX(SomeDudeConcept.WorldWidth / 12);
    %groundPlane.setDefaultFriction(SomeDudeConcept.ObstacleFriction);
    %groundPlane.setCollisionGroups(none);
    SomeDudeConcept.GroundPlane = %groundPlane;
    echo("Initialized Random Terrain Generation");
    }

function SomeDudeConcept::generateTerrain(%this)
    { // Go through and generate points for the terrain (high and low Y values) at regular intervals
    echo("Beginning Random Terrain Generation");
    %ground = SomeDudeConcept.Ground; // Pull in the ground array, start off with our first portion of the terrain, then start adding the hill generation after that
    %ground.add(0, SomeDudeConcept.WorldLeft SPC SomeDudeConcept.FloorLevel + 3.25);
    %ground.add(1, SomeDudeConcept.WorldLeft + (SomeDudeConcept.TileSizeX * 4) SPC SomeDudeConcept.FloorLevel + 3.25);
    %currentPoint = 2;
    %currentX = SomeDudeConcept.WorldLeft + (SomeDudeConcept.TileSizeX * 4); // Set our starting X point
    %currentY = SomeDudeConcept.FloorLevel + 3.25; // Set our starting Y point
    %currentOffsetY = SomeDudeConcept.GroundPointOffsetY;

    while (%currentX <= (SomeDudeConcept.WorldLeft + (SomeDudeConcept.TileSizeX * 64)))
        { // Make sure we do not generate terrain past 64 tiles worth of X
        %newX = %currentX + SomeDudeConcept.GroundPointOffsetX; // Grab a new X value via the offset
        %newY = (%currentY + %currentOffsetY + SomeDudeConcept.GroundPointOffsetY + getRandom(0, SomeDudeConcept.TileSizeY)) * -1;
        %ground.add(%currentPoint, %newX SPC %newY);
        %currentX = %newX;
        %currentY = %newY;
        %currentPoint++;
        echo("Generated Terrain point:" SPC %currentPoint-1 SPC %currentX SPC %currentY);
        }
    echo("Terrain Generation completed");
    %obj = SomeDudeConcept.GroundPlane;
    SomeDudeConcept.createEdgeCollisions(%obj, %ground);
    %obj.CollisionCallback = true;
    %obj.setAwake(false);
    SandboxScene.add(%obj);
}

function SomeDudeConcept::addGroundPoint(%this, %point)
    { // Adds a new point to the ground collision array
    %nCount = SomeDudeConcept.Ground.Count(); // Get a count of the current number of objects in the array
    SomeDudeConcept.Ground.add(%nCount, getWord(%point, 0) SPC getWord(%point, 1)+3.25); // Not sure why we need to 'fudge' this number. It's 3.25 off from the actual position on the screen when we get world position
    SomeDudeConcept.createEdgeCollisions(SomeDudeConcept.GroundPlane, SomeDudeConcept.Ground); // Rebuild the collision shapes
    }

function SomeDudeConcept::popGroundPoint(%this)
    { // Pops the last point off the ground array
    SomeDudeConcept.Ground.pop_back();
    SomeDudeConcept.createEdgeCollisions(SomeDudeConcept.GroundPlane, SomeDudeConcept.Ground); // Rebuild the collision shapes
    }

function SomeDudeConcept::createFloor_old(%this)
    {
    // Ground 
    %obj = new Scroller();
    %obj.setBodyType( "static" );
    //%obj.setImage( "SomeDudeConcept:background_day" );
    %obj.setImage("ToyAssets:blank"); // Make this invisible for now
    %obj.setBlendAlpha(0.0);
    %obj.setSize( SomeDudeConcept.WorldWidth, 3 );
    %obj.setPosition( 0, (SomeDudeConcept.FloorLevel+2.75) - (%obj.getSizeY()/2) );
    %obj.setSceneLayer( SomeDudeConcept.BackgroundDomain); // Need to update this
    %obj.setSceneGroup( SomeDudeConcept.ColGroundDomain );
  
    %obj.setRepeatX( SomeDudeConcept.WorldWidth / 12 );   
    %obj.setDefaultFriction( SomeDudeConcept.ObstacleFriction );
    %obj.setCollisionGroups( none );

    // Collision edge (follows the ground)
    SomeDudeConcept.Ground = new array(); // Create a new array to hold all the ground/edge points
    %ground = SomeDudeConcept.Ground;
    SomeDudeConcept.GroundPlane = %obj; // Store the object so we can retrieve it later

    // These are our old, default values expressed here for testing
    %ground.add(0, SomeDudeConcept.WorldLeft SPC SomeDudeConcept.FloorLevel+3.25);
    %ground.add(1, SomeDudeConcept.WorldLeft+(SomeDudeConcept.TileSizeX*4) SPC SomeDudeConcept.FloorLevel+3.25);
    %ground.add(2, SomeDudeConcept.WorldLeft+(SomeDudeConcept.TileSizeX*5) SPC SomeDudeConcept.FloorLevel+3.25+(SomeDudeConcept.TileSizeX*1));
    %ground.add(3, SomeDudeConcept.WorldLeft+(SomeDudeConcept.TileSizeX*15) SPC SomeDudeConcept.FloorLevel+3.25+(SomeDudeConcept.TileSizeX*1));
    %ground.add(4, SomeDudeConcept.WorldLeft+(SomeDudeConcept.TileSizeX*19) SPC SomeDudeConcept.FloorLevel+3.25-(SomeDudeConcept.TileSizeX*3));
    %ground.add(5, SomeDudeConcept.WorldLeft+(SomeDudeConcept.TileSizeX*23) SPC SomeDudeConcept.FloorLevel+3.25-(SomeDudeConcept.TileSizeX*3));
    %ground.add(6, SomeDudeConcept.WorldLeft+(SomeDudeConcept.TileSizeX*25) SPC SomeDudeConcept.FloorLevel+3.25-(SomeDudeConcept.TileSizeX*1));
    %ground.add(7, SomeDudeConcept.WorldLeft+(SomeDudeConcept.TileSizeX*28) SPC SomeDudeConcept.FloorLevel+3.25-(SomeDudeConcept.TileSizeX*1));
    %ground.add(8, SomeDudeConcept.WorldLeft+(SomeDudeConcept.TileSizeX*29) SPC SomeDudeConcept.FloorLevel+3.25-(SomeDudeConcept.TileSizeX*2));
    %ground.add(9, SomeDudeConcept.WorldLeft+(SomeDudeConcept.TileSizeX*31) SPC SomeDudeConcept.FloorLevel+3.25-(SomeDudeConcept.TileSizeX*2));
    %ground.add(10, SomeDudeConcept.WorldLeft+(SomeDudeConcept.TileSizeX*36) SPC SomeDudeConcept.FloorLevel+3.25+(SomeDudeConcept.TileSizeX*3));
    %ground.add(11, SomeDudeConcept.WorldLeft+(SomeDudeConcept.TileSizeX*38) SPC SomeDudeConcept.FloorLevel+3.25+(SomeDudeConcept.TileSizeX*3));
    %ground.add(12, SomeDudeConcept.WorldLeft+(SomeDudeConcept.TileSizeX*39) SPC SomeDudeConcept.FloorLevel+3.25+(SomeDudeConcept.TileSizeX*4));
    %ground.add(13, SomeDudeConcept.WorldLeft+(SomeDudeConcept.TileSizeX*42) SPC SomeDudeConcept.FloorLevel+3.25+(SomeDudeConcept.TileSizeX*4));
    %ground.add(14, SomeDudeConcept.WorldLeft+(SomeDudeConcept.TileSizeX*46) SPC SomeDudeConcept.FloorLevel+3.25+(SomeDudeConcept.TileSizeX*0));
    %ground.add(15, SomeDudeConcept.WorldLeft+(SomeDudeConcept.TileSizeX*54) SPC SomeDudeConcept.FloorLevel+3.25+(SomeDudeConcept.TileSizeX*0));
    %ground.add(16, SomeDudeConcept.WorldLeft+(SomeDudeConcept.TileSizeX*55) SPC SomeDudeConcept.FloorLevel+3.25-(SomeDudeConcept.TileSizeX*1));
    %ground.add(17, SomeDudeConcept.WorldLeft+(SomeDudeConcept.TileSizeX*64) SPC SomeDudeConcept.FloorLevel+3.25-(SomeDudeConcept.TileSizeX*1));  

    SomeDudeConcept.createEdgeCollisions(%obj, %ground);
    %obj.CollisionCallback = true;
    %obj.setAwake(false);
    SandboxScene.add(%obj);
    }

function SomeDudeConcept::createEdgeCollisions(%this, %object, %array)
    { // Basic function that goes through an %array of points, and creates edge collisions for %object
    
    // Remove any existing collision shapes first
    %nCount = %object.getCollisionShapeCount(); // Get number of collision shapes
    while (%nCount > 0)
        {
        echo("Deleting collision shape" SPC %nCount SPC "from" SPC %object);
        %object.deleteCollisionShape(%nCount-1);
        %nCount--;
        }

    %nCount = 0;
    %nMax = %array.count(); // Get a count of how many points are in the array
    while (%nCount < %nMax-1)
        {
        // Create a collision between the current point (nCount) and the next (nCount+1)
        %object.createEdgeCollisionShape(%array.getValue(%nCount), %array.getValue(%nCount+1));
        %nCount++;
        }
    }