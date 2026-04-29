// Plugins/MusicLibraryPlugin.cs
using System.ComponentModel;
using System.Text.Json;
using Microsoft.SemanticKernel;

public class MusicLibraryPlugin
{
    // Atributo [KernelFunction] marca este método como uma ferramenta para a IA.
    // Atributo [Description] diz à IA para que serve esta ferramenta.
    [KernelFunction, Description("Add a song to a user's recently played list")]
    public static string AddToRecentlyPlayed(
        [Description("The name of the song")] string song,
        [Description("The song's artist")] string artist,
        [Description("The song's genre")] string genre)
    {
        // 1. Lê o arquivo JSON atual
        //string path = Path.Combine(Directory.GetCurrentDirectory(), "Data", "recentlyplayed.txt");
        string path = @"C:\ProjetosC\MusicAiPluginProj\MusicAIPlugin\Data\recentlyplayed.txt";
        var songList = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(File.ReadAllText(path)) ?? new();

        // 2. Adiciona a nova música
        var newSong = new Dictionary<string, string>
        {
            ["title"] = song,
            ["artist"] = artist,
            ["genre"] = genre
        };
        songList.Add(newSong);

        // 3. Salva o arquivo atualizado
        File.WriteAllText(path, JsonSerializer.Serialize(songList, new JsonSerializerOptions { WriteIndented = true }));

        return $"Successfully added '{song}' by {artist} to recently played.";
    }
}