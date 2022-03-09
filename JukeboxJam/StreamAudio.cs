using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;


public class StreamAudio
{
    public enum StreamingPlaybackState
    {
        Stopped,
        Playing,
        Buffering,
        Paused
    }

    private static BufferedWaveProvider bufferedWaveProvider;
    private IWavePlayer waveOut;
    public static StreamingPlaybackState playbackState;
    private static volatile bool fullyDownloaded;
    private VolumeWaveProvider16 volumeProvider;
    IMp3FrameDecompressor decompressor = null;
    Stream stream = null;

    public StreamAudio(Stream stream)
    {
        this.stream = stream;
    }

    public void StreamMusic(object state)
    {
        var buffer = new byte[16384 * 4];

        try
        {
            stream.Read(buffer, 0, buffer.Length);
            var readFullyStream = new ReadFullyStream(stream);
            do
            {
                Thread.Sleep(10);
                if (IsBufferNearlyFull)
                {
                    Debug.WriteLine("Buffer is full!");
                    Thread.Sleep(500);
                }
                else
                {
                    Mp3Frame frame;
                    try
                    {
                        frame = Mp3Frame.LoadFromStream(readFullyStream);
                    }
                    catch (EndOfStreamException)
                    {
                        Debug.WriteLine("EndOfStreamException thrown!");
                        fullyDownloaded = true;
                        break;
                    }

                    if (frame == null)
                        break;

                    if (decompressor == null)
                    {
                        decompressor = CreateFrameDecompressor(frame);
                        bufferedWaveProvider = new BufferedWaveProvider(decompressor.OutputFormat);
                        bufferedWaveProvider.BufferDuration = TimeSpan.FromSeconds(20);
                    }

                    int decompressed = decompressor.DecompressFrame(frame, buffer, 0);
                    bufferedWaveProvider.AddSamples(buffer, 0, decompressed);
                }
            } while (playbackState != StreamingPlaybackState.Stopped);
            decompressor.Dispose();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
        finally
        {
            if (decompressor != null)
            {
                decompressor.Dispose();
            }
        }
    }

    private static bool IsBufferNearlyFull
    {
        get
        {
            return bufferedWaveProvider != null &&
                   bufferedWaveProvider.BufferLength - bufferedWaveProvider.BufferedBytes
                   < bufferedWaveProvider.WaveFormat.AverageBytesPerSecond / 4;
        }
    }

    private static IMp3FrameDecompressor CreateFrameDecompressor(Mp3Frame frame)
    {
        WaveFormat waveFormat = new Mp3WaveFormat(frame.SampleRate, frame.ChannelMode == ChannelMode.Mono ? 1 : 2,
            frame.FrameLength, frame.BitRate);
        return new AcmMp3FrameDecompressor(waveFormat);
    }
    private static IWavePlayer CreateWaveOut()
    {
        return new WaveOut();
    }

    public void Pause()
    {
        if (playbackState == StreamingPlaybackState.Playing || playbackState == StreamingPlaybackState.Buffering)
        {
            if (waveOut != null)
                waveOut.Pause();
            Debug.WriteLine(String.Format("User requested Pause, waveOut.PlaybackState={0}", waveOut.PlaybackState));
            playbackState = StreamingPlaybackState.Paused;
        }
    }

    public void Stop()
    {
        StopPlayback();
    }

    public void StopPlayback()
    {
        if (playbackState != StreamingPlaybackState.Stopped)
        {
            if (!fullyDownloaded)
            {
                //client.CancelPendingRequests();
            }

            playbackState = StreamingPlaybackState.Stopped;
            if (waveOut != null)
            {
                waveOut.Stop();
                waveOut.Dispose();
                waveOut = null;
            }

            Thread.Sleep(500);
        }
    }

    public void UpdateAudioState()
    {
        if (playbackState != StreamingPlaybackState.Stopped)
        {
            if (waveOut == null && bufferedWaveProvider != null)
            {
                waveOut = CreateWaveOut();
                waveOut.PlaybackStopped += OnPlaybackStopped;
                volumeProvider = new VolumeWaveProvider16(bufferedWaveProvider);
                volumeProvider.Volume = 2;
                waveOut.Init(volumeProvider);
                //progressBarBuffer.Maximum = (int)bufferedWaveProvider.BufferDuration.TotalMilliseconds;
            }
            else if (bufferedWaveProvider != null)
            {
                var bufferedSeconds = bufferedWaveProvider.BufferedDuration.TotalSeconds;
                //ShowBufferState(bufferedSeconds);
                // make it stutter less if we buffer up a decent amount before playing
                if (bufferedSeconds < 0.5 && playbackState == StreamingPlaybackState.Playing && !fullyDownloaded)
                {
                    Pause();
                }
                else if (bufferedSeconds > 4 && playbackState == StreamingPlaybackState.Buffering)
                {
                    Play();
                }
                else if (fullyDownloaded && bufferedSeconds == 0)
                {
                    StopPlayback();
                }
            }
        }
    }

    private static void OnPlaybackStopped(object sender, StoppedEventArgs e)
    {
        Debug.WriteLine("Playback Stopped!");
        if (e.Exception != null)
        {
            Debug.WriteLine(e.ToString());
        }
    }

    public void Play()
    {
        try
        {
            waveOut.Play();
            Debug.WriteLine(String.Format("Started playing, waveOut.PlaybackState={0}", waveOut.PlaybackState));
            playbackState = StreamingPlaybackState.Playing;
        } 
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
    }

    public void StartBuffering()
    {
        if (playbackState == StreamingPlaybackState.Stopped)
        {
            playbackState = StreamingPlaybackState.Buffering;
            bufferedWaveProvider = null;
            ThreadPool.QueueUserWorkItem(StreamMusic);
        }
        else if (playbackState == StreamingPlaybackState.Paused)
        {
            playbackState = StreamingPlaybackState.Buffering;
        }
        else
        {
            Pause();
        }
    }
}
