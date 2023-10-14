﻿
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;


namespace AgOpenGPS
{
    public class CTool
    {
        private readonly FormGPS mf;

        public double width, halfWidth,contourWidth;
        public double farLeftPosition = 0;
        public double farLeftSpeed = 0;
        public double farRightPosition = 0;
        public double farRightSpeed = 0;

        public double overlap;
        public double trailingHitchLength, tankTrailingHitchLength;
        public double offset;

        public double lookAheadOffSetting, lookAheadOnSetting;
        public double turnOffDelay;

        public double lookAheadDistanceOnPixelsLeft, lookAheadDistanceOnPixelsRight;
        public double lookAheadDistanceOffPixelsLeft, lookAheadDistanceOffPixelsRight;

        public bool isToolTrailing, isToolTBT;
        public bool isToolRearFixed, isToolFrontFixed;

        public bool isMultiColoredSections, isSectionOffWhenOut;
        public string toolAttachType;

        public double hitchLength;

        //how many individual sections
        public int numOfSections;

        //used for super section off on
        public int minCoverage;

        public bool areAllSectionBtnsOn = true;

        public bool isLeftSideInHeadland = true, isRightSideInHeadland = true, isSectionsNotZones;

        //read pixel values
        public int rpXPosition;
        public int rpWidth;

        public Color[] secColors = new Color[16];

        public int zones;
        public int[] zoneRanges = new int[9];

        //Constructor called by FormGPS
        public CTool(FormGPS _f)
        {

            mf = _f;

            //from settings grab the vehicle specifics
            width = Properties.Settings.Default.setVehicle_toolWidth;
            overlap = Properties.Settings.Default.setVehicle_toolOverlap;

            offset = Properties.Settings.Default.setVehicle_toolOffset;

            trailingHitchLength = Properties.Settings.Default.setTool_toolTrailingHitchLength;
            tankTrailingHitchLength = Properties.Settings.Default.setVehicle_tankTrailingHitchLength;
            hitchLength = Properties.Settings.Default.setVehicle_hitchLength;

            isToolRearFixed = Properties.Settings.Default.setTool_isToolRearFixed;
            isToolTrailing = Properties.Settings.Default.setTool_isToolTrailing;
            isToolTBT = Properties.Settings.Default.setTool_isToolTBT;
            isToolFrontFixed = Properties.Settings.Default.setTool_isToolFront;

            lookAheadOnSetting = Properties.Settings.Default.setVehicle_toolLookAheadOn;
            lookAheadOffSetting = Properties.Settings.Default.setVehicle_toolLookAheadOff;
            turnOffDelay = Properties.Settings.Default.setVehicle_toolOffDelay;

            isSectionOffWhenOut = Properties.Settings.Default.setTool_isSectionOffWhenOut;

            isSectionsNotZones = Properties.Settings.Default.setTool_isSectionsNotZones;
            
            if (isSectionsNotZones) 
                numOfSections = Properties.Settings.Default.setVehicle_numSections;
            else
                numOfSections = Properties.Settings.Default.setTool_numSectionsMulti;

            minCoverage = Properties.Settings.Default.setVehicle_minCoverage;
            isMultiColoredSections = Properties.Settings.Default.setColor_isMultiColorSections;

            secColors[0] =  Properties.Settings.Default.setColor_sec01.CheckColorFor255();
            secColors[1] =  Properties.Settings.Default.setColor_sec02.CheckColorFor255();
            secColors[2] =  Properties.Settings.Default.setColor_sec03.CheckColorFor255();
            secColors[3] =  Properties.Settings.Default.setColor_sec04.CheckColorFor255();
            secColors[4] =  Properties.Settings.Default.setColor_sec05.CheckColorFor255();
            secColors[5] =  Properties.Settings.Default.setColor_sec06.CheckColorFor255();
            secColors[6] =  Properties.Settings.Default.setColor_sec07.CheckColorFor255();
            secColors[7] =  Properties.Settings.Default.setColor_sec08.CheckColorFor255();
            secColors[8] =  Properties.Settings.Default.setColor_sec09.CheckColorFor255();
            secColors[9] =  Properties.Settings.Default.setColor_sec10.CheckColorFor255();
            secColors[10] = Properties.Settings.Default.setColor_sec11.CheckColorFor255();
            secColors[11] = Properties.Settings.Default.setColor_sec12.CheckColorFor255();
            secColors[12] = Properties.Settings.Default.setColor_sec13.CheckColorFor255();
            secColors[13] = Properties.Settings.Default.setColor_sec14.CheckColorFor255();
            secColors[14] = Properties.Settings.Default.setColor_sec15.CheckColorFor255();
            secColors[15] = Properties.Settings.Default.setColor_sec16.CheckColorFor255();

            string[] words = Properties.Settings.Default.setTool_zones.Split(',');
            zones = int.Parse(words[0]);
            for (int i = 0; i < words.Length; i++)
            {
                zoneRanges[i] = int.Parse(words[i]);
            }
        }

        public void DrawTool()
        {
            //translate and rotate at pivot axle
            GL.Translate(mf.pivotAxlePos.easting, mf.pivotAxlePos.northing, 0);
            GL.PushMatrix();

            //translate down to the hitch pin
            GL.Translate(Math.Sin(mf.fixHeading) * hitchLength,
                            Math.Cos(mf.fixHeading) * hitchLength, 0);

            //settings doesn't change trailing hitch length if set to rigid, so do it here
            double trailingTank, trailingTool;
            if (isToolTrailing)
            {
                trailingTank = tankTrailingHitchLength;
                trailingTool = trailingHitchLength;
            }
            else { trailingTank = 0; trailingTool = 0; }

            //there is a trailing tow between hitch
            if (isToolTBT && isToolTrailing)
            {
                //rotate to tank heading
                GL.Rotate(glm.toDegrees(-mf.tankPos.heading), 0.0, 0.0, 1.0);

                //draw the tank hitch
                GL.LineWidth(6);
                //draw the rigid hitch
                GL.Color3(0, 0, 0);
                GL.Begin(PrimitiveType.LineLoop);
                GL.Vertex3(-0.57, trailingTank, 0);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(0.57, trailingTank, 0);

                GL.End();

                GL.LineWidth(1);
                //draw the rigid hitch
                GL.Color3(0.765f, 0.76f, 0.32f);
                GL.Begin(PrimitiveType.LineLoop);
                GL.Vertex3(-0.57, trailingTank, 0);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(0.57, trailingTank, 0);

                GL.End();

                //GL.LineWidth(4);
                //GL.Color3(0.937f, 0.537f, 0.397f);
                //GL.Begin(PrimitiveType.Lines);
                //GL.Vertex3(0, trailingTank, 0);
                //GL.Vertex3(0, 0, 0);
                //GL.End();

                //GL.Color3(0.937f, 0.537f, 0.397f);
                //GL.Begin(PrimitiveType.Lines);
                //GL.Vertex3(-1, trailingTank, 0);
                //GL.Vertex3(1, trailingTank, 0);
                //GL.End();

                //pivot markers
                //GL.Color3(0,0,0);
                //GL.PointSize(8);
                //GL.Begin(PrimitiveType.Points);
                //GL.Vertex3(-1, trailingTank, 0.0);
                //GL.Vertex3(1, trailingTank, 0.0);
                //GL.End();

                //move down the tank hitch, unwind, rotate to section heading
                GL.Translate(0.0, trailingTank, 0.0);
                GL.Rotate(glm.toDegrees(mf.tankPos.heading), 0.0, 0.0, 1.0);
                GL.Rotate(glm.toDegrees(-mf.toolPos.heading), 0.0, 0.0, 1.0);
            }

            //no tow between hitch
            else
            {
                GL.Rotate(glm.toDegrees(-mf.toolPos.heading), 0.0, 0.0, 1.0);
            }

            //draw the hitch if trailing
            if (isToolTrailing)
            {
                GL.LineWidth(6);
                GL.Color3(0, 0, 0);
                GL.Begin(PrimitiveType.LineStrip);
                GL.Vertex3(-0.4 + mf.tool.offset, trailingTool, 0);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(0.4 + mf.tool.offset, trailingTool, 0);

                GL.End();

                GL.LineWidth(1);
                //draw the rigid hitch
                GL.Color3(0.7f, 0.4f, 0.2f);
                GL.Begin(PrimitiveType.LineStrip);
                GL.Vertex3(-0.4 + mf.tool.offset, trailingTool, 0);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(0.4 + mf.tool.offset, trailingTool, 0);

                GL.End();
            }

            if (mf.isJobStarted)
            {
                //look ahead lines
                GL.LineWidth(1);
                GL.Begin(PrimitiveType.Lines);

                //lookahead section on
                GL.Color3(0.20f, 0.7f, 0.2f);
                GL.Vertex3(mf.tool.farLeftPosition, (mf.tool.lookAheadDistanceOnPixelsLeft) * 0.1 + trailingTool, 0);
                GL.Vertex3(mf.tool.farRightPosition, (mf.tool.lookAheadDistanceOnPixelsRight) * 0.1 + trailingTool, 0);

                //lookahead section off
                GL.Color3(0.70f, 0.2f, 0.2f);
                GL.Vertex3(mf.tool.farLeftPosition, (mf.tool.lookAheadDistanceOffPixelsLeft) * 0.1 + trailingTool, 0);
                GL.Vertex3(mf.tool.farRightPosition, (mf.tool.lookAheadDistanceOffPixelsRight) * 0.1 + trailingTool, 0);

                if (mf.vehicle.isHydLiftOn)
                {
                    GL.Color3(0.70f, 0.2f, 0.72f);
                    GL.Vertex3(mf.section[0].positionLeft, (mf.vehicle.hydLiftLookAheadDistanceLeft * 0.1) + trailingTool, 0);
                    GL.Vertex3(mf.section[mf.tool.numOfSections - 1].positionRight, (mf.vehicle.hydLiftLookAheadDistanceRight * 0.1) + trailingTool, 0);
                }

                GL.End();
            }
            //draw the sections
            GL.LineWidth(2);

            double hite = mf.camera.camSetDistance / -150;
            if (hite > 0.7) hite = 0.7;
            if (hite < 0.5) hite = 0.5;

            {
                for (int j = 0; j < numOfSections; j++)
                {
                    //if section is on, green, if off, red color
                    if (mf.section[j].isSectionOn)
                    {
                        if (mf.section[j].sectionBtnState == btnStates.Auto)
                        {
                            //GL.Color3(0.0f, 0.9f, 0.0f);
                            if (mf.section[j].isMappingOn) GL.Color3(0.0f, 0.95f, 0.0f);
                            else GL.Color3(0.970f, 0.30f, 0.970f);
                        }
                        else GL.Color3(0.97, 0.97, 0);
                    }
                    else
                    {
                        if (!mf.section[j].isMappingOn) GL.Color3(0.950f, 0.2f, 0.2f);
                        else GL.Color3(0.00f, 0.250f, 0.97f);
                        //GL.Color3(0.7f, 0.2f, 0.2f);
                    }

                    double mid = (mf.section[j].positionRight - mf.section[j].positionLeft) / 2 + mf.section[j].positionLeft;

                    GL.Begin(PrimitiveType.TriangleFan);
                    {
                        GL.Vertex3(mf.section[j].positionLeft, trailingTool, 0);
                        GL.Vertex3(mf.section[j].positionLeft, trailingTool - hite, 0);

                        GL.Vertex3(mid, trailingTool - hite * 1.5, 0);

                        GL.Vertex3(mf.section[j].positionRight, trailingTool - hite, 0);
                        GL.Vertex3(mf.section[j].positionRight, trailingTool, 0);
                    }
                    GL.End();

                    if (mf.camera.camSetDistance > -150)
                    {
                        GL.Begin(PrimitiveType.LineLoop);
                        {
                            GL.Color3(0.0, 0.0, 0.0);
                            GL.Vertex3(mf.section[j].positionLeft, trailingTool, 0);
                            GL.Vertex3(mf.section[j].positionLeft, trailingTool - hite, 0);

                            GL.Vertex3(mid, trailingTool - hite * 1.5, 0);

                            GL.Vertex3(mf.section[j].positionRight, trailingTool - hite, 0);
                            GL.Vertex3(mf.section[j].positionRight, trailingTool, 0);
                        }
                        GL.End();
                    }
                }

                //zones

                if (!isSectionsNotZones && zones > 0 &&  mf.camera.camSetDistance > -150)
                {
                    //GL.PointSize(8);

                    GL.Begin(PrimitiveType.Lines);
                    for (int i = 1; i < zones; i++)
                    {
                        GL.Color3(0.5f, 0.80f, 0.950f);
                        GL.Vertex3(mf.section[zoneRanges[i]].positionLeft, trailingTool - 0.4, 0);
                        GL.Vertex3(mf.section[zoneRanges[i]].positionLeft, trailingTool + 0.2, 0);
                    }

                    GL.End();
                }


                //tram Dots
                if (mf.tram.displayMode != 0)
                {
                    if (mf.camera.camSetDistance > -200)
                    {

                        if (mf.tram.isOuter)
                        {

                            //section markers
                            GL.Begin(PrimitiveType.Points);

                            //right side
                            if (((mf.tram.controlByte) & 1) == 1) GL.Color3(0.0f, 0.900f, 0.39630f);
                            else GL.Color3(0.90f, 0.00f, 0.0f);
                            GL.Vertex3(farRightPosition - mf.tram.halfWheelTrack, trailingTool + 0.21, 0);

                            //left side
                            if ((mf.tram.controlByte & 2) == 2) GL.Color3(0.0f, 0.900f, 0.3930f);
                            else GL.Color3(0.90f, 0.00f, 0.0f);
                            GL.Vertex3(farLeftPosition + mf.tram.halfWheelTrack, trailingTool + 0.21, 0);
                            GL.End();
                        }
                        else
                        {
                            GL.Begin(PrimitiveType.Points);

                            //right side
                            if (((mf.tram.controlByte) & 1) == 1) GL.Color3(0.0f, 0.900f, 0.39630f);
                            else GL.Color3(0.90f, 0.00f, 0.0f);
                            GL.Vertex3(mf.tram.halfWheelTrack, trailingTool + 0.21, 0);

                            //left side
                            if ((mf.tram.controlByte & 2) == 2) GL.Color3(0.0f, 0.900f, 0.3930f);
                            else GL.Color3(0.90f, 0.00f, 0.0f);
                            GL.Vertex3(-mf.tram.halfWheelTrack, trailingTool + 0.21, 0);
                            GL.End();
                        }
                    }
                }
            }

            //GL.End();

            //draw section markers if close enough
            //if (mf.camera.camSetDistance > -250)
            //{
            //    GL.Color3(0.0f, 0.0f, 0.0f);
            //    //section markers
            //    GL.PointSize(3.0f);
            //    GL.Begin(PrimitiveType.Points);
            //    for (int j = 0; j < numOfSections - 1; j++)
            //        GL.Vertex3(mf.section[j].positionRight, trailingTool, 0);
            //    GL.End();
            //}

            //GL.Color3(0.30f, 1.0f, 1.0f);
            ////section markers
            //GL.PointSize(4.0f);
            //GL.Begin(PrimitiveType.Points);
            ////for (int j = 0; j < numOfSections - 1; j++)
            //GL.Vertex3(mf.section[0].positionLeft, (mf.vehicle.hydLiftLookAheadDistanceLeft * 0.1) + trailingTool, 0);
            //GL.Vertex3(mf.section[mf.tool.numOfSections - 1].positionRight, (mf.vehicle.hydLiftLookAheadDistanceRight * 0.1) + trailingTool, 0);
            //GL.End();

            GL.PopMatrix();
        }
    }
}
