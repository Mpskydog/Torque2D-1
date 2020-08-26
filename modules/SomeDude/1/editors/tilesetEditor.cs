//-----------------------------------------------------------------------------
// tilesetEditor
// A simple tool for editing the tileSet
//-----------------------------------------------------------------------------

function TileMapObject::selectSprite(%this, %touchID, %worldPosition, %type)
{  
    // Pick sprites.
    %sprites = %this.pickPoint( %worldPosition );    

    // Fetch sprite count.    
    %spriteCount = %sprites.count;
    
    // Finish if no sprites picked.
    if ( %spriteCount == 0 )
        return;    
        
    // Iterate sprites.
    for( %i = 0; %i < %spriteCount; %i++ )
    {
        // Fetch sprite Id.
        %spriteId = getWord( %sprites, %i );
        
        // Select the sprite Id.
        %this.selectSpriteId( %spriteId );
        
        %maxFrame = SomeDudeConcept.TileSetCount;

        /*%init = %this.init[%spriteId];
        if (%init $= "")
            {
            // Need to initialize this sprite object
            %this.setSpriteImage("SomeDudeConcept:BasicGroundTile", 0);
            %this.init[%spriteId] = true;
            }*/
        echo("Type of" SPC %type);
        // Increment the image for that sprite
        %currFrame = %this.getSpriteImageFrame(); // Pull in the current frame
        if (%type == 0) // Left-click to increment the sprite frame
            {
            %nextFrame = %currFrame+1;
            if (%nextFrame > %maxFrame)
                %this.setSpriteImageFrame(0); // Set it back to 0
            else
                %this.setSpriteImageFrame(%nextFrame); // Set it to the next frame
            }
        else if (%type == 1) // Right-click to go to previous frame
            {
            %nextFrame = %currFrame-1;
            if (%nextFrame < 0)
                %this.setSpriteImageFrame(%maxFrame); // Set it back to max
            else
                %this.setSpriteImageFrame(%nextFrame); // Set it to the next frame
            }
        /*else if (%type == 1) // Right-click to rotate the image
            {
            %currAngle = %this.getSpriteAngle();
            %currAngle = %currAngle+90.0; // Add 90% to the angle
            if (%currAngle >= 360.0)
                %currAngle = %currAngle-360.0;
            %this.setSpriteAngle(%currAngle); // Update the rotation of the sprite
            }*/
    }
}

function TileMapObject::onTouchDown(%this, %touchID, %worldPosition)
{
    if (SomeDudeConcept.StartUp $= "tile_editor")
        { // Tile editor
        %this.selectSprite(%touchID, %worldPosition, 0); // Left mouse
        echo("TileMapLeftDown");
        }
}

function TileMapObject::purgeEmptyTiles(%this, %frame)
{ // Cycle through all sprites in the composite sprite - any who have a frame that is equal to blank
// are deleted
%count = %this.getSpriteCount(); // Get total sprites in the composite
%currSprite = 1;
while (%currSprite <= %count)
    {
    %this.selectSpriteId(%currSprite);
    if (%this.getSpriteImageFrame() == %frame)
        %this.removeSprite();
    %currSprite++;
    }    
}

function SomeDudeConcept::createTilemap_editor(%this)
{
    SandboxWindow.setUseObjectInputEvents(true); // Make sure we pass input events on to objects
    %Tilemap = new CompositeSprite()
        {
        Class = "TileMapObject";
        };
    %Tilemap.setDefaultSpriteStride(SomeDudeConcept.TileSizeX, SomeDudeConcept.TileSizeY);
    %Tilemap.setDefaultSpriteSize(SomeDudeConcept.TileSizeX, SomeDudeConcept.TileSizeY);

    %Tilemap.SetBatchLayout( "rect" );

    // Set the batch sort mode for when we're render isolated.
    %Tilemap.SetBatchSortMode( "z" );

    // Set the batch render isolation.
    %Tilemap.SetBatchIsolated( SomeDudeConcept.RenderIsolated );
    //%Tilemap.setPosition(SomeDudeConcept.TilePositionX, SomeDudeConcept.TilePositionY);
    %Tilemap.setPosition("-215.05 -28.357");
    //%Tilemap.setSceneLayer(SomeDudeConcept.BackgroundDomain);
    %Tilemap.setSceneLayer(0); // We put it on top only because we want to see all the stuff while editing
    %Tilemap.setUseInputEvents(true); // Make sure its set to use input events
    %Tilemap.gravityScale = 0; // no gravity
    %Tilemap.LayoutMode = "Rectilinear";
    
    // Initialize the tilemap
    %maxX = SomeDudeConcept.TileCountX-1;
    %maxY = SomeDudeConcept.TileCountY-1;
    %x = 0;
    %y = 0;
    	
	// Add some sprites.
    for ( %y = 0; %y <= %maxY; %y++ )
	{
	    for ( %x = 0; %x <= %maxX; %x++ )
        {                
            // Add a sprite with the specified logical position.
	        // In rectilinear layout this two-part position is scaled by the default sprite-stride.
            %Tilemap.addSprite( %x SPC %y );
            //%Tilemap.setSpriteSize(SomeDudeConcept.TileSizeX, SomeDudeConcept.TileSizeY);
            echo("Added a tile");
            // The sprite is automatically selected when it is added so
            // we can perform operations on it immediately.        
                        
            // Set the sprite image with a default, blank frame
            echo(%x SPC %y);
            %Tilemap.setSpriteImage( "SomeDudeConcept:basicRoomTileset", 5 );  
        }
	}

    SandboxScene.add(%Tilemap);
    SomeDudeConcept.TileMap = %Tilemap;
}