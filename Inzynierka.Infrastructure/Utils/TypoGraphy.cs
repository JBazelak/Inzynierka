using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Inzynierka.Infrastructure.Utils
{
    public static class TypoGraphy
    {
        public static TextStyle Normal => TextStyle
            .Default
            .FontFamily("Roboto Flex")
            .FontColor("#000000");
    }
}
