//-----------------------------------------------------------------------------
// Contains functions/code related specifically to the custom editors
// Calls the other editor files
//-----------------------------------------------------------------------------

// Camera functions
//----------------------------------------------------------------
exec("./groundEditor.cs");
exec("./tilesetEditor.cs");
exec("./objectEditor.cs");


function cameraUp()
{
    $cameraMove = true;
    SomeDudeConcept.cameraMove("up");
}

function cameraStop()
{
    $cameraMove = false;
}

function cameraDown()
{
    $cameraMove = true;
    SomeDudeConcept.cameraMove("down");
}

function cameraLeft()
{
    $cameraMove = true;
    SomeDudeConcept.cameraMove("left");
}

function cameraRight()
{
    $cameraMove = true;
    SomeDudeConcept.cameraMove("right");
}

function SomeDudeConcept::cameraMove(%this, %direction)
{
    if ($cameraMove == false)
        {
        return;
        }
    %posY = SandboxWindow.getCameraPosition().y;
    %posX = SandboxWindow.getCameraPosition().x;
    if (%direction $= "up")
        %posY = %posY+2;
    if (%direction $= "down")
        %posY = %posY-2;
    if (%direction $= "left")
        %posX = %posX-2;
    if (%direction $= "right")
        %posX = %posX+2;

    SandboxWindow.setCameraPosition(%posX SPC %posY);
    SomeDudeConcept.schedule(100, "cameraMove", %direction);
}