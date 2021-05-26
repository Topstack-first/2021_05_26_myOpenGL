using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace OpenGL
{
    public class ChessDraw
    {
		/* State Variables */
		public static int angle = 0;

		public static int th = 0; // Azimuthal anGL.gle

		public static int ph = 40; // Elevation of View

		public static int zh = 0; // Azimuth of light
		public static double ypos = 20; // yposition of light;
		public static double dim = 150; // dimension of view box
		public static int fov = 45; // Field of view for perspective mode
		public static double asp = 1; // Aspect ratio
		public static int white = 0; // Variable for piece color
		public static int black = 1; // Variable for piece clor
		public static int move = 1; // State of light movement
									// Load textures

		public static uint[] piecetexture = { LoadTexBMP("whitemarble.bmp") , LoadTexBMP("blackmarble.bmp") }; // Holds piece textures
		public static uint[] boardtexture = { LoadTexBMP("white_wood.bmp"), LoadTexBMP("black_wood.bmp"), LoadTexBMP("red_wood.bmp") }; // holds textures for the board
		public static float[] Position = { 60 , 20, 0, 0.5f };


		public static double sin(double degrees)
        {
			double angle = Math.PI * degrees / 180.0;
			return Math.Sin(angle);
		}
		public static double cos(double degrees)
		{
			double angle = Math.PI * degrees / 180.0;
			return Math.Cos(angle);
		}
		/* CHESS PIECE BUILDING BLOCKS */

		/*
		 * Draw a Vertex in polar coordinates and
		 * bind a texture coordinate
		 * */
		public static void polarVertex(double th, double ph)
		{
			double x = -sin(th) * cos(ph);
			double y = cos(th) * cos(ph);
			double z = sin(ph);
			GL.glNormal3d(x, y, z);
			GL.glTexCoord2d(th / 360.0, ph / 180.0 + 0.5);
			GL.glVertex3d(x, y, z);
		}

		/* Draw a pyramid at (x, y, z)
		 *  Define texture coordinates and normals for lighting
		 *  Order of vertices for face culling
		 * */
		public static void drawPyramid(double x, double y, double z)
		{
			GL.glPushMatrix();
			GL.glTranslated(x, y, z);
			GL.glBegin(GL.GL_TRIANGLES);
			GL.glNormal3d(-1, 2, 0);
			GL.glTexCoord2d(0.5, 1.0); GL.glVertex3f(0, 2, 0); ;
			GL.glTexCoord2d(1.0, 0.0); GL.glVertex3f(-1, 0, -1);
			GL.glTexCoord2d(0.0, 0.0); GL.glVertex3f(-1, 0, 1);
			GL.glNormal3d(0, 2, -1);
			GL.glTexCoord2d(0.5, 1.0); GL.glVertex3f(0, 2, 0); ;
			GL.glTexCoord2d(0.0, 1.0); GL.glVertex3f(1, 0, -1);
			GL.glTexCoord2d(1.0, 0.0); GL.glVertex3f(-1, 0, -1);
			GL.glNormal3d(1, 2, 0);
			GL.glTexCoord2d(0.5, 1.0); GL.glVertex3f(0, 2, 0);
			GL.glTexCoord2d(1.0, 0.0); GL.glVertex3f(1, 0, 1); ;
			GL.glTexCoord2d(0.0, 1.0); GL.glVertex3f(1, 0, -1);
			GL.glNormal3d(0, 2, 1);
			GL.glTexCoord2d(1.0, 0.0); GL.glVertex3f(1, 0, 1); ;
			GL.glTexCoord2d(0.5, 1.0); GL.glVertex3f(0, 2, 0); ;
			GL.glTexCoord2d(0.0, 1.0); GL.glVertex3f(-1, 0, 1);
			GL.glEnd();
			GL.glPopMatrix();
		}

		/*
		 * Draw a Rectangular Box at (x,y,z) with dimensions (dx,dy,dz)
		 * */
		public static void drawCube(double x, double y, double z, double dx, double dy, double dz)
		{
			// Save the transformation
			GL.glPushMatrix();
			// Apply the offset
			GL.glTranslated(x, y, z);
			GL.glScaled(dx, dy, dz);
			// Draw the rectangular prism
			GL.glBegin(GL.GL_QUADS);
			// Front
			GL.glNormal3d(0, 0, 1);
			GL.glTexCoord2d(0, 0); GL.glVertex3f(-1, -1, 1);
			GL.glTexCoord2d(0, 1); GL.glVertex3f(1, -1, 1);
			GL.glTexCoord2d(1, 1); GL.glVertex3f(1, 1, 1);
			GL.glTexCoord2d(1, 0); GL.glVertex3f(-1, 1, 1);
			// Back
			GL.glNormal3d(0, 0, -1);
			GL.glTexCoord2d(0, 0); GL.glVertex3f(1, -1, -1);
			GL.glTexCoord2d(0, 1); GL.glVertex3f(-1, -1, -1);
			GL.glTexCoord2d(1, 1); GL.glVertex3f(-1, 1, -1);
			GL.glTexCoord2d(1, 0); GL.glVertex3f(1, 1, -1);
			// Right
			GL.glNormal3d(1, 0, 0);
			GL.glTexCoord2d(0, 0); GL.glVertex3f(1, -1, 1);
			GL.glTexCoord2d(0, 1); GL.glVertex3f(1, -1, -1);
			GL.glTexCoord2d(1, 0); GL.glVertex3f(1, 1, -1);
			GL.glTexCoord2d(1, 1); GL.glVertex3f(1, 1, 1);
			// Left
			GL.glNormal3d(-1, 0, 0);
			GL.glTexCoord2d(0, 0); GL.glVertex3f(-1, -1, -1);
			GL.glTexCoord2d(0, 1); GL.glVertex3f(-1, -1, 1);
			GL.glTexCoord2d(1, 0); GL.glVertex3f(-1, 1, 1);
			GL.glTexCoord2d(1, 1); GL.glVertex3f(-1, 1, -1);
			// Top
			GL.glNormal3d(0, 1, 0);
			GL.glTexCoord2d(0, 0); GL.glVertex3f(-1, 1, 1);
			GL.glTexCoord2d(0, 1); GL.glVertex3f(1, 1, 1);
			GL.glTexCoord2d(1, 1); GL.glVertex3f(1, 1, -1);
			GL.glTexCoord2d(1, 0); GL.glVertex3f(-1, 1, -1);
			// Botton
			GL.glNormal3d(0, -1, 0);
			GL.glTexCoord2d(0, 0); GL.glVertex3f(-1, -1, -1);
			GL.glTexCoord2d(0, 1); GL.glVertex3f(1, -1, -1);
			GL.glTexCoord2d(1, 1); GL.glVertex3f(1, -1, 1);
			GL.glTexCoord2d(1, 0); GL.glVertex3f(-1, -1, 1);
			// End
			GL.glEnd();
			// Undo transformations
			GL.glPopMatrix();
		}

		/*
		 *  Draw a sphere at (x, y, z) with radius (r)
		 */
		public static void drawSphere(double x, double y, double z, double r)
		{
			const int d = 5;
			int th, ph;
			//  Save transformation
			GL.glPushMatrix();
			//  Offset and scale
			GL.glTranslated(x, y, z);
			GL.glScaled(r, r, r);
			//  Latitude bands
			for (ph = -90; ph < 90; ph += d)
			{
				GL.glBegin(GL.GL_QUAD_STRIP);
				for (th = 0; th <= 360; th += d)
				{
					polarVertex(th, ph + d); // This vertex first for face culling
					polarVertex(th, ph);
				}
				GL.glEnd();
			}
			//  Undo transformations
			GL.glPopMatrix();
		}

		/*
		 *  Draw a hemisphere at (x, y, z) with radius (r)
		 */
		public static void drawHemisphere(double x, double y, double z, double r)
		{
			const int d = 5;
			int th, ph;
			//  Save transformation
			GL.glPushMatrix();
			//  Offset and scale
			GL.glTranslated(x, y, z);
			GL.glRotated(270, 0, 0, 1);
			GL.glScaled(r, r, r);
			//  Latitude bands
			for (ph = -90; ph < 90; ph += d)
			{
				GL.glBegin(GL.GL_QUAD_STRIP);
				for (th = 0; th <= 180; th += d)
				{
					polarVertex(th, ph + d); // This vertex first for face culling
					polarVertex(th, ph);
				}
				GL.glEnd();
			}
			//  Undo transformations
			GL.glPopMatrix();
		}

		/* Draw a Cylinder of radius r and height y */
		public static void drawCylinder(double r, double y)
		{
			GL.glPushMatrix();
			GL.glRotated(90, 1, 0, 0);
			GL.glScaled(r, r, y);
			int i;
			/* Top of the Cylinder */
			GL.glNormal3f(0, 0, 1);
			GL.glBegin(GL.GL_TRIANGLE_FAN);
			GL.glTexCoord2f(0.5f, 0.5f);
			GL.glVertex3f(0, 0, 1);
			for (i = 0; i <= 360; i += 10)
			{
				GL.glTexCoord2f(0.5f * (float)cos(i) + 0.5f, 0.5f * (float)sin(i) + 0.5f);
				GL.glVertex3f((float)cos(i), (float)sin(i), 1);
			}
			GL.glEnd();

			/* Bottom of the Cylinder */
			GL.glNormal3f(0, 0, -1);
			GL.glBegin(GL.GL_TRIANGLE_FAN);
			GL.glTexCoord2f((float)0.5, (float)0.5);
			GL.glVertex3f(0, 0, -1);
			for (i = 0; i <= 360; i += 10)
			{
				GL.glTexCoord2f((float)0.5 * (float)cos(i) + (float)0.5, (float)0.5 * (float)sin(i) + (float)0.5);
				GL.glVertex3f(-1 * (float)cos(i), (float)sin(i), -1);
			}
			GL.glEnd();
			/* Side of Cylinder */
			GL.glBegin(GL.GL_QUADS);
			double sidediv = 45;
			for (i = 0; i <= 360; i += (int)sidediv)
			{
				GL.glNormal3f((float)cos(i), (float)sin(i), 0);
				GL.glTexCoord2f(1, 1); GL.glVertex3f((float)cos(i + sidediv), (float)sin(i + sidediv), +1);
				GL.glTexCoord2f(1, 0); GL.glVertex3f((float)cos(i), (float)sin(i), +1);
				GL.glTexCoord2f(0, 0); GL.glVertex3f((float)cos(i), (float)sin(i), -1);
				GL.glTexCoord2f(0, 1); GL.glVertex3f((float)cos(i + sidediv), (float)sin(i + sidediv), -1);
			}
			GL.glEnd();
			GL.glPopMatrix();
		}

		/*
		 * Draw a curved cone (hat-like shape)
		 * eccentricity (ecc) varies the severity of the curve
		 * height (height) varies the height of the base
		 * This will act as a base for many pieces
		 * */
		public static void drawCurvedBase(double ecc, double height)
		{
			// Save the transformation
			GL.glPushMatrix();
			GL.glRotated(180, 1, 0, 0);
			// Apply the offset
			double i; // iteration variable
			/* Time Step */
			double dt = 0.2;
			/* Draw several cylinders with radii of the function
			 * (ecc^x) to create a curved survace */
			for (i = dt; i < height; i = i + dt)
			{
				GL.glTranslated(0, .2, 0);
				double radius = Math.Pow(ecc, i);
				drawCylinder(radius, .2);
			}
			GL.glPopMatrix();
			//GL.glutPostRedisplay();
		}

		/* BUILDING THE CHESS PIECES*/
		/*
		 * Draw a pawn at location (x, y, z) with color (color)
		 * */
		public static void drawPawn(double x, double y, double z, int color)
		{
			/* Set up GL.gl textures */
			GL.glEnable(GL.GL_TEXTURE_2D);
			GL.glTexEnvi(GL.GL_TEXTURE_ENV, GL.GL_TEXTURE_ENV_MODE, (int)GL.GL_MODULATE);
			GL.glBindTexture(GL.GL_TEXTURE_2D, piecetexture[color]);
			// Save the transformation matrix
			GL.glPushMatrix();
			GL.glTranslated(x, y, z);
			GL.glPushMatrix();
			GL.glScaled(1, 4, 1);
			/* Translate the piece up by its height to set the base at 0 */
			GL.glTranslated(0, 2.1, 0);
			// Draw the base of the pawn
			drawCurvedBase(2, 2);
			GL.glPopMatrix();
			/* Draw the Head of the Pawn */
			/* Translate the head up to the height * the yscale */
			GL.glTranslated(0, 8, 0);
			drawSphere(0, 0, 0, 1.7);
			GL.glPopMatrix();
			GL.glDisable(GL.GL_TEXTURE_2D);
		}
		/* Draw a Rook at location (x, y, z) with color (color)
		 * */
		public static void drawRook(double x, double y, double z, int color)
		{
			GL.glEnable(GL.GL_TEXTURE_2D);
			GL.glTexEnvi(GL.GL_TEXTURE_ENV, GL.GL_TEXTURE_ENV_MODE, (int)GL.GL_MODULATE);
			GL.glBindTexture(GL.GL_TEXTURE_2D, piecetexture[color]);
			GL.glPushMatrix();
			GL.glTranslated(x, y, z);
			GL.glScaled(2.2, 4, 2.2);
			// Draw the base of the rook
			/* Translate the base up to 0 */
			GL.glTranslated(0, 2.5, 0);
			drawCurvedBase(1.1, 2.5);
			// Draw the top
			drawCylinder(1.5, 0.6);
			// draw the battlements
			int i;
			for (i = 0; i < 360; i = i + 60)
			{
				drawCube(1.5 * cos(i), 0.4, 1.5 * sin(i), .3, .7, .3);
			}
			GL.glPopMatrix();
			GL.glDisable(GL.GL_TEXTURE_2D);
		}
		/* Draw a Bishop at location (x, y, z) with color (color)
		 * */
		public static void drawBishop(double x, double y, double z, int color)
		{
			GL.glEnable(GL.GL_TEXTURE_2D);
			GL.glTexEnvi(GL.GL_TEXTURE_ENV, GL.GL_TEXTURE_ENV_MODE, (int)GL.GL_MODULATE);
			GL.glBindTexture(GL.GL_TEXTURE_2D, piecetexture[color]);

			GL.glPushMatrix();
			GL.glTranslated(x, y, z);
			GL.glScaled(0.6, 3, 0.6);
			/* Translate the base up to 0 */
			GL.glTranslated(0, 2.6, 0);
			// Draw the base of the bishop
			drawCurvedBase(2, 2.6);
			// Draw the stand of the bishop
			GL.glTranslated(0, -1.8, 0);
			drawCylinder(1.3, 0.7);
			// Draw the collar of the bishop
			GL.glTranslated(0, 2, 0);
			drawCylinder(2, 0.2);
			GL.glScaled(2.8, 0.8, 2.8);
			drawSphere(0, 1, 0, 0.8);
			drawSphere(0, 1.8, 0, 0.4);
			GL.glDisable(GL.GL_TEXTURE_2D);
			GL.glPopMatrix();
		}

		/* Draw a Queen at location (x, y, z) with color (color)
		 * */
		public static void drawQueen(double x, double y, double z, int color)
		{
			GL.glEnable(GL.GL_TEXTURE_2D);
			GL.glTexEnvi(GL.GL_TEXTURE_ENV, GL.GL_TEXTURE_ENV_MODE, (int)GL.GL_MODULATE);
			GL.glBindTexture(GL.GL_TEXTURE_2D, piecetexture[color]);
			GL.glPushMatrix();
			GL.glTranslated(x, y, z);

			GL.glScaled(1, 4, 1);
			/* Translate the base up to 0 */
			GL.glTranslated(0, 3, 0);
			// Draw the base of the queen
			drawCurvedBase(1.3, 3);

			// Draw the collar of the queen
			drawCylinder(2, 0.2);

			// Draw the Queen's crown
			GL.glScaled(1, 0.4, 1);
			double i; // iteration variable
			for (i = 0.2; i < 1.5; i = i + 0.05)
			{
				GL.glTranslated(0, 0.05, 0);
				drawCylinder(i, 0.05);
			}
			for (i = 1.5; i > 0; i = i - 0.05)
			{
				GL.glTranslated(0, 0.02, 0);
				drawCylinder(i, 0.05);
			}
			drawSphere(0, 0.15, 0, 0.25);
			for (i = 0; i < 360; i = i + 60)
			{
				drawSphere(1.5 * cos(i), 0, 1.5 * sin(i), 0.15);
			}
			GL.glPopMatrix();
			GL.glDisable(GL.GL_TEXTURE_2D);
		}
		/* Draw a King at location (x, y, z) with color (color)
		 * */
		public static void drawKing(double x, double y, double z, int color)
		{
			GL.glEnable(GL.GL_TEXTURE_2D);
			GL.glTexEnvi(GL.GL_TEXTURE_ENV, GL.GL_TEXTURE_ENV_MODE, (int)GL.GL_MODULATE);
			GL.glBindTexture(GL.GL_TEXTURE_2D, piecetexture[color]);
			GL.glPushMatrix();
			GL.glTranslated(x, y, z);
			/* Translate the base up to 0 */
			GL.glTranslated(0, 12.8, 0);
			GL.glScaled(1.3, 4, 1.3);
			// Draw the base of the King
			drawCurvedBase(1.2, 3.2);
			// Draw the king's collar
			drawCylinder(1.5, 0.3);
			// Draw the King's crown
			double i; // iteration variable
			for (i = 0.8; i < 1; i = i + 0.02)
			{
				GL.glTranslated(0, 0.1, 0);
				drawCylinder(i, 0.1);
			}
			for (i = 1; i > 0; i = i - 0.05)
			{
				GL.glTranslated(0, 0.02, 0);
				drawCylinder(i, 0.05);
			}
			// Draw a cross on top of the crown
			GL.glScaled(1, 0.4, 1);
			drawCube(0, 0.5, 0, 0.1, 0.5, 0.1);
			drawCube(0, 0.6, 0, 0.3, 0.1, 0.1);
			GL.glPopMatrix();
			GL.glDisable(GL.GL_TEXTURE_2D);
		}
		/* Draw a knight at the location (x, y, z) with color (color)
		 * */
		public static void drawKnight(double x, double y, double z, int color)
		{
			GL.glEnable(GL.GL_TEXTURE_2D);
			GL.glTexEnvi(GL.GL_TEXTURE_ENV, GL.GL_TEXTURE_ENV_MODE, (int)GL.GL_MODULATE);
			GL.glBindTexture(GL.GL_TEXTURE_2D, piecetexture[color]);
			GL.glPushMatrix();
			GL.glTranslated(x, y, z);
			GL.glTranslated(0, 6.75, 0);
			GL.glRotated(180, 0, 1, 0);
			GL.glPushMatrix();
			// Draw the Nose
			GL.glTranslated(0, 3, 0);
			GL.glRotated(245, 1, 0, 0);
			GL.glScaled(1.95, 4, 1.95);
			drawHemisphere(0, 0, 0, 0.6);
			GL.glPopMatrix();
			GL.glPushMatrix();
			// Draw the head
			GL.glTranslated(0, 3, 0);
			drawSphere(0, 0, 0, 1.3);
			GL.glPopMatrix();
			// Draw the ears of the horse
			GL.glPushMatrix();
			GL.glTranslated(0, 3, 0);
			GL.glRotated(-30, 1, 0, 0);
			GL.glScaled(0.2, 0.9, 0.2);
			drawPyramid(2.4, 0.8, 0.4);
			drawPyramid(-2.4, 0.8, 0.4);
			GL.glPopMatrix();
			// draw the body of the horse
			double neck = 2.5; // iteration variables
			int ang = 50;
			GL.glPushMatrix();
			GL.glTranslated(0, -6, 0);
			GL.glRotated(-90, 0, 1, 0);
			GL.glScaled(0.5, 0.5, 0.5);
			// Draw the base of the knight
			/* Curvature of the neck follows Sin */
			drawCylinder(7, 1.5);
			while (ang < 280)
			{
				GL.glTranslated(sin(ang) / 15, .15, 0);
				ang = ang + 2;
				drawCylinder(neck + sin(ang), 1);
			}
			GL.glPopMatrix();
			GL.glPopMatrix();
			GL.glDisable(GL.GL_TEXTURE_2D);
		}

		/* Set up one side of the chess board */
		public static void drawSet(int color)
		{
			/* Set up a row of pawns */
			int i;
			int spacing = 10;
			for (i = 0; i < 8; i++)
			{
				drawPawn((i - 4) * spacing, 0, 10, color);
			}
			/* Set the rooks */
			drawRook(-4 * spacing, 0, 0, color);
			drawRook(3 * spacing, 0, 0, color);
			/* Set the knights */
			drawKnight(-3 * spacing, 0, 0, color);
			drawKnight(2 * spacing, 0, 0, color);
			/* Set the bishops*/
			drawBishop(-2 * spacing, 0, 0, color);
			drawBishop(1 * spacing, 0, 0, color);
			/* Set the King and Queen */
			if (color == 0)
			{
				drawQueen(-spacing, 0, 0, color);
				drawKing(0, 0, 0, color);
			}
			else
			{
				drawQueen(0, 0, 0, color);
				drawKing(-spacing, 0, 0, color);
			}
		}

		/* Draw a chess board for the pieces. */
		public static void drawBoard()
		{
			GL.glEnable(GL.GL_TEXTURE_2D);
			GL.glTexEnvi(GL.GL_TEXTURE_ENV, GL.GL_TEXTURE_ENV_MODE, (int)GL.GL_MODULATE);
			int color = 1;
			GL.glPushMatrix();
			GL.glRotated(90, 1, 0, 0);
			int i, k;
			int spacing = 10; // Size of each checker
			/* Render the 64 checkers */
			for (i = -4 * spacing; i <= 3 * spacing; i += spacing)
			{
				for (k = -4 * spacing; k <= 3 * spacing; k += spacing)
				{
					GL.glBindTexture(GL.GL_TEXTURE_2D, boardtexture[color]);
					drawCube(i, k, 0, spacing / 2, spacing / 2, 0.5);
					color = 1 - color; // Alternate checker color
				}
				color = 1 - color; // Alternate checker color
			}
			// place a border around the board
			GL.glBindTexture(GL.GL_TEXTURE_2D, boardtexture[2]);
			for (i = -5 * spacing; i <= 4 * spacing; i += spacing)
			{
				for (k = -5 * spacing; k <= 4 * spacing; k += spacing)
				{
					drawCube(i, k, 0, spacing / 2, spacing / 2, 0.5);
				}
			}
			GL.glPopMatrix();
		}

		/*
		 * Glut will call this to display the view
		 * Basic structures are not yet drawn in the correct direction to enable face
		 * culling.
		 * */
		public static void display()
		{
			
			// Clear the window and set the depth buffer
			GL.glClear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT);
			// Enable Z-buffering
			GL.glEnable(GL.GL_DEPTH_TEST);
			// Enable face culling 
			GL.glEnable(GL.GL_CULL_FACE);
			// Undo previous transformations	
			GL.glLoadIdentity();
			// Set up position of the eye for GL.gluLookAt
			double Ex = -3 * dim * sin(th) * cos(ph);
			double Ey = 3 * dim * sin(ph);
			double Ez = 3 * dim * cos(th) * cos(ph);
			GLU.gluLookAt(Ex, Ey, Ez, 0, 0, 0, 0, cos(ph), 0);
			// Set up decent viewing scale
			GL.glPushMatrix();
			GL.glScaled(3, 3, 3);
			// Initialize light position
			
			// Draw light position as a small sphere
			GL.glColor3f(1, 1, 1);
			drawSphere(Position[0], Position[1], Position[2], Position[3]);
			// Translate material intensity to color vectors
			float[] Ambient = { 0.3f, 0.3f, 0.3f, 1.0f };
			float[] Diffuse = { 1, 1, 1, 1 };
			float[] Specular = { 0.5f, 0.5f, 0, 0.5f };
			float[] White_V = { 1, 1, 1, 1 };
			// Enable lighting and vector normaliztion
			GL.glEnable(GL.GL_LIGHTING);
			GL.glEnable(GL.GL_NORMALIZE);
			//  GL.glColor sets ambient and diffuse color materials
			GL.glColorMaterial(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE);
			GL.glEnable(GL.GL_COLOR_MATERIAL);
			//  Enable light0
			GL.glEnable(GL.GL_LIGHT0);


			// Set ambient, diffuse, specular components and position of light0
			GL.glLightfv(GL.GL_LIGHT0, GL.GL_AMBIENT, Ambient);
			GL.glLightfv(GL.GL_LIGHT0, GL.GL_DIFFUSE, Diffuse);
			GL.glLightfv(GL.GL_LIGHT0, GL.GL_SPECULAR, Specular);
			GL.glLightfv(GL.GL_LIGHT0, GL.GL_POSITION, Position);
			GL.glMaterialf(GL.GL_FRONT_AND_BACK, GL.GL_SHININESS, 32.0f);
			GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_SPECULAR, White_V);

			// draw the chess board
			GL.glRotated(0+angle, 0, 1, 0);
			drawBoard();


			// draw the white pieces on the board
			GL.glPushMatrix();
			GL.glTranslated(0, 0, -40);
			drawSet(white);
			GL.glPopMatrix();

			// draw the black pieces on the board
			GL.glPushMatrix();
			GL.glTranslated(-10, 0, 30);
			GL.glRotated(180 , 0, 1, 0);
			drawSet(black);
			GL.glPopMatrix();

			/* Render the projected shadow. */

			/* Render 50% black shadow color on top of whatever the
			   floor appareance is. 
			GL.glLoadIdentity();
			GL.glEnable(GL.GL_BLEND);
			GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);
			GL.glDisable(GL.GL_LIGHTING);  // Force the 50% black.
			GL.glColor4f(0.0f, 0.0f, 0.0f, 0.5f);

			GL.glPushMatrix();
			// Project the shadow. 
			float[] shadowMat = new float[16];
			float[] groudplane = { 0, 0, 0, 0.5f };
			shadowMatrix(shadowMat, groudplane, Position);

			GL.glMultMatrixf(shadowMat);
			//drawDinosaur();

			GL.glPopMatrix();

			GL.glDisable(GL.GL_BLEND);
			GL.glEnable(GL.GL_LIGHTING);
			*/

			// Render the scene and make it visible
			GL.glFlush();
			GL.glPopMatrix();
			//GL.glutSwapBuffers();
		}
		public static void shadowMatrix(float[] shadowMat,  float[] groundplane, float[] lightpos)
		{
			float dot;

			/* Find dot product between light position vector and ground plane normal. */
			dot = groundplane[0] * lightpos[0] +
			  groundplane[1] * lightpos[1] +
			  groundplane[2] * lightpos[2] +
			  groundplane[3] * lightpos[3];

			for(int i=0;i<4;i++)
            {
				shadowMat[i] = dot - lightpos[i] * groundplane[0];
				shadowMat[4+i] = 0.0f - lightpos[i] * groundplane[1];
				shadowMat[8+i] = 0.0f - lightpos[i] * groundplane[2];
				shadowMat[12+i] = 0.0f - lightpos[i] * groundplane[3];
			}
		}

		/*
		 * GLUT will call this to handle window resizing
		 * */
		public static void reshape(int width, int height)
		{
			// Ratio of the width to the height of the window
			asp = (height > 0) ? (double)width / height : 1;
			// Set the viewport to the entire window
			GL.glViewport(0, 0, width, height);
			// Reset the projection
			Project(fov, asp, dim);
		}
		public static void Project(double fov, double asp, double dim)
		{
			//  Tell OpenGL we want to manipulate the projection matrix
			GL.glMatrixMode(GL.GL_PROJECTION);
			//  Undo previous transformations
			GL.glLoadIdentity();
			//  Perspective transformation
			if (fov > 0)
				GLU.gluPerspective(fov, asp, dim / 16, 16 * dim);
			//  Orthogonal transformation
			else
				GL.glOrtho(-asp * dim, asp * dim, -dim, +dim, -dim, +dim);
			//  Switch to manipulating the model matrix
			GL.glMatrixMode(GL.GL_MODELVIEW);
			//  Undo previous transformations
			GL.glLoadIdentity();
		}
		/*
		 *  Load texture from BMP file
		 */
		public static uint LoadTexBMP(string file)
		{
			Bitmap bitmap = new Bitmap(file);

			uint tex;
			GL.glHint(GL.GL_PERSPECTIVE_CORRECTION_Hint, GL.GL_NICEST);

			uint[] textures = new uint[1];
			GL.glGenTextures(1, textures);

			tex = textures[0];
			GL.glBindTexture(GL.GL_TEXTURE_2D, (uint)tex);

			BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
				ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			GL.glTexImage2D(GL.GL_TEXTURE_2D, 0, 3, data.Width, data.Height, 0,
				GL.GL_BGRA_EXT, GL.GL_UNSIGNED_byte,data.Scan0);
			bitmap.UnlockBits(data);


			GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MAG_FILTER,(int)GL.GL_LINEAR);
			GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MIN_FILTER,(int)GL.GL_LINEAR);

			return tex;
		}

	}
}
