using UnityEngine;
using System.IO;

namespace workspace.YU__FFE.utils {


    public static class WavUtility {

        public static byte[] FromAudioClip(AudioClip clip) {
            using (MemoryStream memStream = new MemoryStream()) {
                using (BinaryWriter writer = new BinaryWriter(memStream)) {
                    WriteWavHeader(writer, clip);
                    WriteAudioData(writer, clip);
                }
                return memStream.ToArray();
            }
        }

        private static void WriteWavHeader(BinaryWriter writer, AudioClip clip) {
            int sampleCount = clip.samples * clip.channels;
            int byteRate = 16 * clip.frequency * clip.channels / 8;

            // RIFF Header
            writer.Write(System.Text.Encoding.UTF8.GetBytes("RIFF"));
            writer.Write(36 + sampleCount * 2); // File size - 8 bytes
            writer.Write(System.Text.Encoding.UTF8.GetBytes("WAVE"));

            // Format Chunk
            writer.Write(System.Text.Encoding.UTF8.GetBytes("fmt "));
            writer.Write(16); // Chunk size
            writer.Write((short)1); // Audio format (1 = PCM)
            writer.Write((short)clip.channels);
            writer.Write(clip.frequency);
            writer.Write(byteRate);
            writer.Write((short)(clip.channels * 2)); // Block align
            writer.Write((short)16); // Bits per sample

            // Data Chunk
            writer.Write(System.Text.Encoding.UTF8.GetBytes("data"));
            writer.Write(sampleCount * 2); // Data size
        }

        private static void WriteAudioData(BinaryWriter writer, AudioClip clip) {
            float[] samples = new float[clip.samples * clip.channels];
            clip.GetData(samples, 0);

            for (int i = 0; i < samples.Length; i++) {
                short sample = (short)(samples[i] * short.MaxValue);
                writer.Write(sample);
            }
        }
    }

}
