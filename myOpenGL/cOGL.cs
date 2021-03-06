using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace OpenGL
{
    class cOGL
    {
        Control p;
        int Width;
        int Height;

        public float fovy = 45.0f;
        public float aspect = 1.0f;
        public float zNear = 1.0f;
        public float zFar = 1000.0f;

        public cOGL(Control pb)
        {
            p=pb;
            Width = p.Width;
            Height = p.Height; 
            InitializeGL();
        }

        ~cOGL()
        {
            WGL.wglDeleteContext(m_uint_RC);
        }

		uint m_uint_HWND = 0;
        public uint HWND
		{
			get{ return m_uint_HWND; }
		}
		
        uint m_uint_DC   = 0;

        public uint DC
		{
			get{ return m_uint_DC;}
		}
		uint m_uint_RC   = 0;

        public uint RC
		{
			get{ return m_uint_RC; }
		}

        protected void DrawAll()
        {
            //axes
            GL.glBegin(GL.GL_LINES);
            //x  RED
            GL.glColor3f(1.0f, 0.0f, 0.0f);
            GL.glVertex3f(0.0f, 0.0f, 0.0f);
            GL.glVertex3f(3.0f, 0.0f, 0.0f);
            //y  GREEN 
            GL.glColor3f(0.0f, 1.0f, 0.0f);
            GL.glVertex3f(0.0f, 0.0f, 0.0f);
            GL.glVertex3f(0.0f, 3.0f, 0.0f);
            //z  BLUE
            GL.glColor3f(0.0f, 0.0f, 1.0f);
            GL.glVertex3f(0.0f, 0.0f, 0.0f);
            GL.glVertex3f(0.0f, 0.0f, 3.0f);

            GL.glEnd();

            // cube
            GL.glBegin(GL.GL_QUADS);


            //1

            GL.glColor3f(0.0f, 0.0f, 0.0f);
            GL.glVertex3f(0.0f, 0.0f, 0.0f);

            GL.glColor3f(0.0f, 1.0f, 0.0f);
            GL.glVertex3f(0.0f, 1.0f, 0.0f);

            GL.glColor3f(1.0f, 1.0f, 0.0f);
            GL.glVertex3f(1.0f, 1.0f, 0.0f);

            GL.glColor3f(1.0f, 0.0f, 0.0f);
            GL.glVertex3f(1.0f, 0.0f, 0.0f);

            //2

            GL.glColor3f(0.0f, 0.0f, 0.0f);
            GL.glVertex3f(0.0f, 0.0f, 0.0f);

            GL.glColor3f(0.0f, 0.0f, 1.0f);
            GL.glVertex3f(0.0f, 0.0f, 1.0f);

            GL.glColor3f(0.0f, 1.0f, 1.0f);
            GL.glVertex3f(0.0f, 1.0f, 1.0f);

            GL.glColor3f(0.0f, 1.0f, 0.0f);
            GL.glVertex3f(0.0f, 1.0f, 0.0f);


            //3

            GL.glColor3f(0.0f, 0.0f, 0.0f);
            GL.glVertex3f(0.0f, 0.0f, 0.0f);

            GL.glColor3f(1.0f, 0.0f, 0.0f);
            GL.glVertex3f(1.0f, 0.0f, 0.0f);

            GL.glColor3f(1.0f, 0.0f, 1.0f);
            GL.glVertex3f(1.0f, 0.0f, 1.0f);

            GL.glColor3f(0.0f, 0.0f, 1.0f);
            GL.glVertex3f(0.0f, 0.0f, 1.0f);


            //4

            GL.glColor3f(1.0f, 0.0f, 0.0f);
            GL.glVertex3f(1.0f, 0.0f, 0.0f);

            GL.glColor3f(1.0f, 0.0f, 1.0f);
            GL.glVertex3f(1.0f, 0.0f, 1.0f);

            GL.glColor3f(1.0f, 1.0f, 1.0f);
            GL.glVertex3f(1.0f, 1.0f, 1.0f);

            GL.glColor3f(1.0f, 1.0f, 0.0f);
            GL.glVertex3f(1.0f, 1.0f, 0.0f);


            //5

            GL.glColor3f(1.0f, 1.0f, 1.0f);
            GL.glVertex3f(1.0f, 1.0f, 1.0f);

            GL.glColor3f(1.0f, 1.0f, 0.0f);
            GL.glVertex3f(1.0f, 1.0f, 0.0f);

            GL.glColor3f(0.0f, 1.0f, 0.0f);
            GL.glVertex3f(0.0f, 1.0f, 0.0f);

            GL.glColor3f(0.0f, 1.0f, 1.0f);
            GL.glVertex3f(0.0f, 1.0f, 1.0f);


            //6

            GL.glColor3f(1.0f, 1.0f, 1.0f);
            GL.glVertex3f(1.0f, 1.0f, 1.0f);

            GL.glColor3f(0.0f, 1.0f, 1.0f);
            GL.glVertex3f(0.0f, 1.0f, 1.0f);

            GL.glColor3f(0.0f, 0.0f, 1.0f);
            GL.glVertex3f(0.0f, 0.0f, 1.0f);

            GL.glColor3f(1.0f, 0.0f, 1.0f);
            GL.glVertex3f(1.0f, 0.0f, 1.0f);


            GL.glEnd();
        }
 
        float angle = 0.0f;

        public void Draw()
        {
            if (m_uint_DC == 0 || m_uint_RC == 0)
                return;

            //GL.glClear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT);
            //GL.glLoadIdentity();


            //GL.glTranslatef(0.0f, 0.0f, -6.0f);						// Translate 6 Units Into The Screen

            //angle += 5.0f;
            //GL.glRotatef(angle, 1.0f, 2.0f, 3.0f);

            //DrawAll();

            ChessDraw.display();

            GL.glFlush();

            WGL.wglSwapBuffers(m_uint_DC);

        }

		protected virtual void InitializeGL()
		{
			m_uint_HWND = (uint)p.Handle.ToInt32();
			m_uint_DC   = WGL.GetDC(m_uint_HWND);

            // Not doing the following WGL.wglSwapBuffers() on the DC will
			// result in a failure to subsequently create the RC.
			WGL.wglSwapBuffers(m_uint_DC);

			WGL.PIXELFORMATDESCRIPTOR pfd = new WGL.PIXELFORMATDESCRIPTOR();
			WGL.ZeroPixelDescriptor(ref pfd);
			pfd.nVersion        = 1; 
			pfd.dwFlags         = (WGL.PFD_DRAW_TO_WINDOW |  WGL.PFD_SUPPORT_OPENGL |  WGL.PFD_DOUBLEBUFFER); 
			pfd.iPixelType      = (byte)(WGL.PFD_TYPE_RGBA);
			pfd.cColorBits      = 32;
			pfd.cDepthBits      = 32;
			pfd.iLayerType      = (byte)(WGL.PFD_MAIN_PLANE);

			int pixelFormatIndex = 0;
			pixelFormatIndex = WGL.ChoosePixelFormat(m_uint_DC, ref pfd);
			if(pixelFormatIndex == 0)
			{
				MessageBox.Show("Unable to retrieve pixel format");
				return;
			}

			if(WGL.SetPixelFormat(m_uint_DC,pixelFormatIndex,ref pfd) == 0)
			{
				MessageBox.Show("Unable to set pixel format");
				return;
			}
			//Create rendering context
			m_uint_RC = WGL.wglCreateContext(m_uint_DC);
			if(m_uint_RC == 0)
			{
				MessageBox.Show("Unable to get rendering context");
				return;
			}
			if(WGL.wglMakeCurrent(m_uint_DC,m_uint_RC) == 0)
			{
				MessageBox.Show("Unable to make rendering context current");
				return;
			}


            initRenderingGL();
        }

        public void OnResize()
        {
            
            Width = p.Width;
            Height = p.Height;

            //!!!!!!!
            GL.glViewport(0, 0, Width, Height);
            //!!!!!!!
            
            initRenderingGL();
            Draw();
            
        }

        protected virtual void initRenderingGL()
		{
			if(m_uint_DC == 0 || m_uint_RC == 0)
				return;
			if(this.Width == 0 || this.Height == 0)
				return;


            GL.glEnable(GL.GL_DEPTH_TEST);
            GL.glEnable(GL.GL_CULL_FACE);


            GL.glViewport(0, 0, this.Width, this.Height);

            GL.glClearColor(0, 0, 0, 0); 
			GL.glMatrixMode ( GL.GL_PROJECTION );
			GL.glLoadIdentity();

            aspect = ((float)Width) / Height;
            GLU.gluPerspective( fovy,aspect, zNear,zFar);
            
            GL.glMatrixMode ( GL.GL_MODELVIEW );
			GL.glLoadIdentity();

            
        }
    }
}
