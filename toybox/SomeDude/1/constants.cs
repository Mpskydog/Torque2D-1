//-----------------------------------------------------------------------------
/* Contains a variety of constants for the game, sorted by object type/category
 * Changing these constants here will change the values for every place they are
 * used */
//-----------------------------------------------------------------------------

// Game build/version information
//----------------------------------------------------------------
SomeDudeConcept.GameRelease = "alpha"; // release
SomeDudeConcept.GameBuild = "1 2 0"; // build

// Game mode/start up behavior
// --------------------------------------------------------------
/* These can be set/uncommented in order to change the start up behavior. If one of them is true, the regular game loading phases will not take
 * place and the controls/inputs will be overwritten. For these modes, the arrow keys move the camera along the X and Y axis instead of mounting
 * it to the truck  */
//SomeDudeConcept.StartUp = "tile_editor";
//SomeDudeConcept.StartUp = "ground_editor";
//SomeDudeConcept.StartUp = "object_editor";

//SomeDudeConcept.terrainGeneration = true; // Uncommenting this will cause the game to generate random, hilly terrain rather than loading the terrain from a file

// Uncomment these to build a Talm Schema file
//$Pref::T2D::TAMLSchema = "Torque2d.xsd";
//GenerateTamlSchema();
//quit();

// General objects stuff
//-----------------------------------------------------------------
SomeDudeConcept.FrictionFurniture = 0.75;
SomeDudeConcept.FrictionBoxes = 0.8;
SomeDudeConcept.FrictionNonbreakable = 0.2;
SomeDudeConcept.FrictionBalls = 0.5;
SomeDudeConcept.RestitutionFurniture = 0.05;
SomeDudeConcept.RestitutionNonbreakable = 0.2;
SomeDudeConcept.RestitutionBalls = 0.7;    
SomeDudeConcept.RestitutionBoxes = 0.1;    
SomeDudeConcept.DensityFurniture = 1;
SomeDudeConcept.DensityNonbreakable = 0.1;
SomeDudeConcept.DensityBoxes = 2;

// Truck Body
// ----------------------------------------------------------------
SomeDudeConcept.WheelSpeed = 750;// motor speed for the truck wheels at full power
SomeDudeConcept.WheelFriction = 1; // default friction for the wheels
SomeDudeConcept.FrontWheelDensity = 6; // density (used to calculate mass) of the wheels
SomeDudeConcept.RearWheelDensity = 6;
SomeDudeConcept.FrontWheelRestitution = 0.1; // density (used to calculate mass) of the wheels
SomeDudeConcept.RearWheelRestitution = 0.1;

SomeDudeConcept.FrontWheelDrive = true;
SomeDudeConcept.RearWheelDrive = true;
SomeDudeConcept.loadPhase = 2; // Starting load phase
SomeDudeConcept.RotateCamera = false; // Whether to rotate the camera with the truck, or keep it mounted at a fixed position
SomeDudeConcept.FrictionTruckBody = 0.5;
SomeDudeConcept.RestitutionTruckBody = 0.0;
SomeDudeConcept.DensityTruckBody = 2.5;

// Tileset Editor and Constants
//----------------------------------------------------------------------
SomeDudeConcept.TileCountX = 64; // Logical number of individual sprites in the Composite Sprite Object
SomeDudeConcept.TileCountY = 8;
SomeDudeConcept.TileSizeX = 6.827; // Size of the logical sprites in the Composite Sprite Object
SomeDudeConcept.TileSizeY = 6.827;
SomeDudeConcept.TileSetName = "string"; // Base string name for the image/tileset
SomeDudeConcept.TileSetCount = 5; // Total number of separate images in the set. When cycling through them, it will stop at max and roll over to 0
SomeDudeConcept.TilePositionX = -100;
SomeDudeConcept.TilePositionY = (((SomeDudeConcept.TileCountY/2) * SomeDudeConcept.TileSizeY) * -1) - 1;
addNumericOption("Tile Set Position X", -150, 150, 1, "setTilePositionX", SomeDudeConcept.TilePositionX, false, "Sets the position of the Tile Set on the screen.");
addNumericOption("Tile Set Position Y", -50, 50, 1, "setTilePositionY", SomeDudeConcept.TilePositionY, false, "Sets the position of the Tile Set on the screen.");
addNumericOption("Tile Set Count X", 0, 50, 1, "setTileCountX", SomeDudeConcept.TileCountX, false, "Sets the number of tiles per row.");
addNumericOption("Tile Set Count Y", 0, 50, 1, "setTileCountY", SomeDudeConcept.TileCountY, false, "Sets the number of tiles per column.");
addNumericOption("Tile Set Size X", 0, 50, 1, "setTileSizeX", SomeDudeConcept.TileSizeX, false, "Sets the size of the images in the tile.");
addNumericOption("Tile Set Size Y", 0, 50, 1, "setTileSizeY", SomeDudeConcept.TileSizeY, false, "Sets the size of the images in the tile.");
addStringOption("Tile Set Name", 25, "setTileSetName", SomeDudeConcept.TileSetName, false, "Sets the base name of the tileset to be used.");
addNumericOption("Tiles in Set", 0, 100, 1, "setTileSetCount", SomeDudeConcept.TileSetCount, false, "Number of tiles in the tileset to be cycled through.");

// General level/world behavior
// --------------------------------------------------------------
SomeDudeConcept.ObstacleFriction = 0.5; // Default friction for any obstacles
SomeDudeConcept.CameraWidth = 20;
SomeDudeConcept.CameraHeight = 15;
SomeDudeConcept.CameraAngle = 0;
SomeDudeConcept.FloorLevel = -4.5; // A general value for the level of the floor plane, not used as much now that ground is manually created based off the tile set
SomeDudeConcept.WorldWidth = SomeDudeConcept.TileCountX * SomeDudeConcept.TileSizeX; // relies on the tilesize and tilecount information set under Tileset Editor
SomeDudeConcept.WorldLeft = SomeDudeConcept.WorldWidth/-2;
SomeDudeConcept.WorldRight = SomeDudeConcept.WorldWidth/2;
// Camera view limits (prevents the camera from scrolling past or above/below a set limit, for instance to keep it from showing the world past the edge of the tiles
//SomeDudeConcept.ViewLimitLeft = "";
//SomeDudeConcept.ViewLimitTop = "";
//SomeDudeConcept.ViewLimitRight = "";
//SomeDudeConcept.ViewLimitBottom = "";

SomeDudeConcept.GravityX = 0;
SomeDudeConcept.GravityY = -9.8;
SomeDudeConcept.VelocityIterations = 16; // Box2D default = 8
SomeDudeConcept.PositionIterations = 6; // Box2D default = 3
SomeDudeConcept.GroundFriction = 0.75; // default ground friction
SomeDudeConcept.GroundPointOffsetX = (SomeDudeConcept.TileSizeX * 4); // Set our X offset to about 4 tiles worth per hill
SomeDudeConcept.GroundPointOffsetY = (SomeDudeConcept.TileSizeY * 1); // Set our Y offset to about 1 tiles worth per hill
