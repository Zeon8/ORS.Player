using ImGuiNET;
using Microsoft.Xna.Framework;
using MonoGame.ImGuiNet;

namespace ORS.Player
{
    public class DebugScreen
    {
        private readonly OrsPlayer _player;
        private readonly ImGuiRenderer _guiRenderer;

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

            ImGui.End();

            ImGui.Begin("Commands");
            ImGui.BeginChild("Scrolling");
            ImGui.BeginTable("table", 3, ImGuiTableFlags.Borders);
            foreach (var command in _player.LoadedCommands)
            {
                ImGui.TableNextRow();
                ImGui.TableSetColumnIndex(0);
                ImGui.Text(command.StartTime.ToString());
                ImGui.TableSetColumnIndex(1);
                ImGui.Text(command.ToString());
                ImGui.TableSetColumnIndex(2);
                ImGui.Text(command.EndTime.ToString());
            }
            ImGui.EndTable();
            ImGui.EndChild();
            ImGui.End();
            _guiRenderer.EndLayout();
        }
    }
}
