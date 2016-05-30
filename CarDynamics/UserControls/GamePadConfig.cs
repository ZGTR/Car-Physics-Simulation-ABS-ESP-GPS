using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarDynamics
{


    /// <summary>
    /// simlpe class for storing Game Pad presets
    /// </summary>
    public class GamePadConfig
    {
        public bool rotateLeftThumbStick;
        public bool rotateRightThumbStick;
        public bool invertLeftThumbStick;
        public bool invertRightThumbStick;
        public float sensitivityLeftThumbStick;
        public float sensitivityRightThumbStick;
        public int pointOfView;
        public int bStart;
        public int bBack;
        public int bX;
        public int bY;
        public int bA;
        public int bB;
        public int bShoulderLeft;
        public int bShoulderRight;
        public int bTriggerLeft;
        public int bTriggerRight;


        public GamePadConfig()
        {
            rotateLeftThumbStick = false;
            rotateRightThumbStick = false;
            invertLeftThumbStick = true;
            invertRightThumbStick = true;
            sensitivityLeftThumbStick = 0.15f;
            sensitivityRightThumbStick = 0.15f;
            pointOfView = 0;
            bX = 0;
            bY = 1;
            bA = 2;
            bB = 3;
            bBack = 10;
            bStart = 11;
            bShoulderLeft = 4;
            bShoulderRight = 5;
            bTriggerLeft = 6;
            bTriggerRight = 7;
        }
    }



}



/* 
 Copyright (c) 2010, Damjan Stulic of Wicked Smiles Studios
 http://www.wickedsmilesstudios.com/
 
 Contact information:
 damjan[at]wickedsmilesstudios.com
 http://twitter.com/dstulic

Permission is hereby granted, free of charge, to any person
obtaining a copy of this software and associated documentation
files (the "Software"), to deal in the Software without
restriction, including without limitation the rights to use,
copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the
Software is furnished to do so, subject to the following
conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.
 */
