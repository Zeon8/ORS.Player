using ORS.Parser.Commands;

namespace ORS.Parser
{
    public interface ICommandVisitor
    {
        void Visit(BlackFadeCommand command);
        void Visit(CreateBackgroundCommand command);
        void Visit(NextCommand command);
        void Visit(PlayMovieCommand command);
        void Visit(PlaySoundCommand command);
        void Visit(PlayVoiceCommand command);
        void Visit(PrintTextCommand command);
        void Visit(SkipFrameCommand command);
        void Visit(SelectCommand command);
        void Visit(PlayBackgroundCommand playBackgroundCommand);
        void Visit(WhiteFadeCommand command);
    }
}
