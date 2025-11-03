using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using MonoGame.ImGuiNet;

namespace ORS.Player.Components
{
    public class DebugScreen
    {
        private enum ShowOption
        {
            All,
            Running,
            Finished,
        }

        private ShowOption Option => (ShowOption)_option;

        private readonly OrsPlayer _player;
        private readonly ImGuiRenderer _guiRenderer;
        private int _option;

        public DebugScreen(OrsPlayer player, ImGuiRenderer guiRenderer)
        {
            _player = player;
            _guiRenderer = guiRenderer;
        }

        public void Draw(GameTime gameTime)
        {
            _guiRenderer.BeginLayout(gameTime);

            ImGui.Begin("Game");
            ImGui.Text($"Time: {_player.Time}");

            if (ImGui.Button("Reset"))
                _player.Reset();

            if (ImGui.Button(_player.IsPaused ? "Play" : "Pause"))
                _player.IsPaused = !_player.IsPaused;

            SpeedButton(1f);
            SpeedButton(2f);
            SpeedButton(4f);
            SpeedButton(16f);
            SpeedButton(32f);

            ImGui.End();

            ImGui.Begin("Commands");

            ImGui.RadioButton("All", ref _option, (int)ShowOption.All);
            ImGui.RadioButton("Running", ref _option, (int)ShowOption.Running);
            ImGui.RadioButton("Finished", ref _option, (int)ShowOption.Finished);

            ImGui.BeginChild("Scrolling");
            ImGui.BeginTable("table", 3, ImGuiTableFlags.Borders);
            foreach (var command in _player.LoadedCommands)
            {
                if (Option == ShowOption.Running && !command.IsRunning)
                    continue;
                if (Option == ShowOption.Finished && _player.Time < command.EndTime)
                    continue;
                ImGui.TableNextRow();
                ImGui.TableSetColumnIndex(0);
                ImGui.Text(command.ToString());
                ImGui.TableSetColumnIndex(1);
                ImGui.Text((command.EndTime - command.StartTime).ToString());
                ImGui.TableSetColumnIndex(2);
                ImGui.Text((command.RealEndTime - command.RealStartTime).ToString());
            }
            ImGui.EndTable();
            ImGui.EndChild();
            ImGui.End();
            _guiRenderer.EndLayout();
        }

        private void SpeedButton(float speed)
        {
            if (!_player.IsPaused && ImGui.Button($"{speed}x"))
                _player.Speed = speed;
        }
    }
}
