using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PhysicalEngine.Input;
using PhysicalEngine.Graphics;
using System;
using PhysicalEngine.Tools;
using System.Diagnostics;
using PhysicalEngine.Physics;
using System.Collections.Generic;
using System.Linq;

namespace PhysicalEngine;

public class Game1 : Game
{
    public const double UPDATES_PER_SECOND = 60d;

    private GraphicsDeviceManager _graphics;
    private PEScreen _screen;
    private PESprites _sprites;
    private PEShapes _shapes;
    private PECamera _camera;

    private List<PEBody> _bodies;
    private PEWorld _world;
    private List<Color> _colors;
    private List<Color> _colorsOutline;

    public Game1()
    {
         _graphics = new GraphicsDeviceManager(this);
        _graphics.SynchronizeWithVerticalRetrace = true;

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        IsFixedTimeStep = true;

        TargetElapsedTime = TimeSpan.FromTicks((long)Math.Round((double)TimeSpan.TicksPerSecond / UPDATES_PER_SECOND));
    }

    protected override void Initialize()
    {
        base.Initialize();

        _screen = new PEScreen(this, 1000, 600);
        _sprites = new PESprites(this);
        _shapes = new PEShapes(this);
        _camera = new PECamera(_screen);
        _camera.Zoom = 4;
        _camera.GetExtents(out float left, out float right, out float bottom, out float top);
        _colors = new List<Color>();
        _colorsOutline = new List<Color>();
        _bodies = new List<PEBody>();
        _world = new PEWorld();

        for (int i = 0; i < 5; i++)
        {
            PEBody body = null;
            float x = PERandom.RandomSingle(left + 25f, right - 25f);
            float y = PERandom.RandomSingle(bottom + 200f, top + 50f);

            if (PERandom.RandomInteger(0, 2) == 0)
            {
                PEBody.CreateCircleBody(6f, new PEVector2(x, y), 2f, false, 0.5f, out body, out string message);
            }
            else
            {
                PEBody.CreateBoxBody(12f, 12f, new PEVector2(x, y), 2f, false, 0.5f, out body, out string message);
            }

            _bodies.Add(body);
            _colorsOutline.Add(Color.White);
            _colors.Add(new Color(PERandom.RandomSingle(0f, 1f), PERandom.RandomSingle(0f, 1f), PERandom.RandomSingle(0f, 1f)));
            _world.AddBody(body);
        }
    }

    protected override void LoadContent()
    {
    }

    protected override void Update(GameTime gameTime)
    {
        PEKeyboard keyboard = PEKeyboard.Instance;

        keyboard.Update();

        if (keyboard.IsKeyAvailable)
        {
            if (keyboard.IsKeyClicked(Keys.Escape))
            {
                Exit();
            }

            if (keyboard.IsKeyClicked(Keys.Q))
            {
                _camera.IncZoom();
            }

            if (keyboard.IsKeyClicked(Keys.E))
            {
                _camera.DecZoom();
            }

            float dx = 0f;
            float dy = 0f;
            float speed = 30f;

            if (keyboard.IsKeyDown(Keys.A))
            {
                dx --;
            }

            if (keyboard.IsKeyDown(Keys.D))
            {
                dx ++;
            }

            if (keyboard.IsKeyDown(Keys.S))
            {
                dy --;
            }

            if (keyboard.IsKeyDown(Keys.W))
            {
                dy ++;
            }

            _world.GetBody(0, out PEBody body);

            if (dx != 0f || dy != 0f)
            {
                PEVector2 dir = PEMath.Normalize(new PEVector2(dx, dy));
                PEVector2 vel = dir * speed;

                body.AddForce(vel);
            }

            if (keyboard.IsKeyDown(Keys.R))
            {
                body.Rotate(MathF.PI / 2f * PEUtils.GetElapsedTimeInSeconds(gameTime));
            }
        }

        _world.Step(PEUtils.GetElapsedTimeInSeconds(gameTime), 1);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        _screen.Set();

        GraphicsDevice.Clear(Color.Gray);

        _shapes.Begin(_camera);

        for (int i = 0; i < _bodies.Count; i++)
        {
            if (_world.GetBody(i, out PEBody body))
            {
                Vector2 pos = new Vector2(body.Position.X, body.Position.Y);

                if (body.ShapeType == ShapeType.Circle)
                {
                    _shapes.DrawCircleFill(pos, body.Radius, 30, _colors[_bodies.IndexOf(body)]);
                    _shapes.DrawCircle(pos, body.Radius, 30, _colorsOutline[_bodies.IndexOf(body)]);
                }
                else
                {
                    PEVector2[] pePoints = body.GetTransformedVertices();
                    Vector2[] points = new Vector2[pePoints.Length];

                    for (int j = 0; j < pePoints.Length; j++)
                    {
                        points[j] = new Vector2(pePoints[j].X, pePoints[j].Y);
                    }

                    _shapes.DrawPolygonFill(points, body.Triangles, _colors[_bodies.IndexOf(body)]);
                    _shapes.DrawPolygon(points, _colorsOutline[_bodies.IndexOf(body)]);
                }
            }
        }

        _shapes.End();
        _screen.Unset();
        _screen.Present(_sprites);

        base.Draw(gameTime);
    }
}
