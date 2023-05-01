SharpGLES
=========

OpenGL ES 2.0 for .NET Desktop

Use via nuget:

```
PM> Install-Package SharpGLES
```

The are examples in the Examples folder.

To use in Windows Forms:

```
using System;
using System.Windows.Forms;
using SharpGLES;

public partial class GLESForm : Form
{
	private EGLDisplay _display;

	public GLESForm()
	{
		SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
	}

	protected override void OnPaintBackground(PaintEventArgs e)
	{

	}

	protected override void OnPaint(PaintEventArgs e)
	{
		Draw();

		_display.SwapBuffers();

		Invalidate();
	}

	private void Draw()
	{
		GLES20.Viewport(0, 0, Width, Height);

		GLES20.ClearColor(0f, 0f, 0f, 0f);
		GLES20.Clear(GLES20.GL_COLOR_BUFFER_BIT);

		//draw away...
	}

	protected override void OnHandleDestroyed(EventArgs e)
	{
		_display.Dispose();

		base.OnHandleDestroyed(e);
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		base.OnHandleCreated(e);

		_display = new EGLDisplay(Handle);
	}
}
```

The GLBuffer<T> class can be used to send data to the likes of GLES20.BufferData etc as it provides an implicit cast to IntPtr and uses an internally pinned array.

To load a .NET image as a texture use the GLUtils.TexImage2D method.

This project uses binaries from Google's angle project (https://code.google.com/p/angleproject/) under the New BSD license which can be read here:
https://chromium.googlesource.com/angle/angle/+/master/LICENSE
