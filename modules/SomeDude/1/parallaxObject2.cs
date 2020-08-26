//-----------------------------------------------------------------------------
// Torque Game Builder
// Copyright (C) GarageGames.com, Inc.
// Parallax Object Scrolling Behavior Version 1.01
// By Jeremy L. Anderson
// Thanks to Mark Shaerer, Joseph Walters, Guy Lewis, and Magnus Blikstad
// Version 2.0 Object smoothing implemented...now looks clean on use.
//-----------------------------------------------------------------------------

if (!isObject(ParallaxObjectBehavior))
{
   %template = new BehaviorTemplate(ParallaxObjectBehavior);
   
   %template.friendlyName = "Parallax Object";
   %template.behaviorType = "Effects";
   %template.description  = "Changes object position based on camera movement.";

   %template.addBehaviorField(xSpeed, "Percentage of horizontal scroll speed", float, 100);
   %template.addBehaviorField(ySpeed, "Percentage of vertical scroll speed", float, 100);
    echo("behaviorSubstantiated");
}

function ParallaxObjectBehavior::onBehaviorAdd(%this)
{
	%this.owner.startTimer("ParallaxOnUpdate", 50);  
    echo("BehaviorAdded");
}

function ParallaxObjectBehavior::onLevelLoaded(%scenegraph)
{

	%currPos = SandboxWindow.getCameraPosition();
	%this.oldPosX = getWord(%currPos, 0);
	%this.oldPosY = getWord(%currPos, 1);

}

function ParallaxObject::ParallaxOnUpdate(%this)
{
	%currPos = SandboxWindow.getCameraPosition();
	%this.currPosX = getWord(%currPos, 0);
	%this.currPosY = getWord(%currPos, 1);
	%VectorX = ((%this.currPosX - %this.oldPosX)/100)*(100 - %this.xSpeed);
	%VectorY = ((%this.currPosY - %this.oldPosY)/100)*(100 - %this.ySpeed);
	%movSpeed = (mAbs(%VectorX) + mAbs(%VectorY))*100;
	%objPosX = getWord(%this.getPosition(),0);
	%objPosY = getWord(%this.getPosition(),1);
	%targX = %VectorX + %objPosX;
	%targY = %VectorY + %objPosY;
	%this.moveTo(%targX, %targY, %movSpeed);
	
	%this.oldPosX = %this.currPosX;
	%this.oldPosY = %this.currPosY;
    echo("update?");
}