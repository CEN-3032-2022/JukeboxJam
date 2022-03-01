using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using NAudio.Wave;



public class JukeboxClient
{
    static void Main()
    {
        Console.WriteLine("Hi");
        JukeboxClient jc = new JukeboxClient();
        jc.buttonPlay_Click();
        jc.timer1_Tick();
        jc.Play();
    }

    enum StreamingPlaybackState
    {
        Stopped,
        Playing,
        Buffering,
        Paused
    }

    //static readonly HttpClient client = new HttpClient();
    private static BufferedWaveProvider bufferedWaveProvider;
    private IWavePlayer waveOut;
    private static volatile StreamingPlaybackState playbackState;
    private static volatile bool fullyDownloaded;
    private VolumeWaveProvider16 volumeProvider;
    IMp3FrameDecompressor decompressor = null;
    //private HttpWebRequest request;
    //byte[] buffer = File.ReadAllBytes("bensound-sunny.mp3");

    public void StreamAudio(object state)
    {
        /*fullyDownloaded = false;
        request = (HttpWebRequest) WebRequest.Create("http://localhost:80/");
        HttpWebResponse response;

        // get the stream from the server
        try
        {
            response = (HttpWebResponse) request.GetResponse();
        }
        catch(Exception ex) 
        {
            Console.WriteLine(ex.Message);
            return;
        }*/

        var buffer = new byte[16384 * 4];

        try
        {
            // tests
            FileStream iStream = new FileStream("bensound-sunny.mp3", FileMode.Open, FileAccess.Read, FileShare.Read);
            var readFullyStream = new ReadFullyStream(iStream);
            do
            {
                if (IsBufferNearlyFull)
                {
                    Console.WriteLine("Buffer is full!");
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
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            if(decompressor != null)
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
            waveOut.Pause();
            Console.WriteLine(String.Format("User requested Pause, waveOut.PlaybackState={0}", waveOut.PlaybackState));
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

    private void timer1_Tick()
    {
        if (playbackState != StreamingPlaybackState.Stopped)
        {
            if (waveOut == null && bufferedWaveProvider != null)
            {
                //Debug.WriteLine("Creating WaveOut Device");
                waveOut = CreateWaveOut();
                waveOut.PlaybackStopped += OnPlaybackStopped;
                volumeProvider = new VolumeWaveProvider16(bufferedWaveProvider);
                volumeProvider.Volume = 70;
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
                    //Debug.WriteLine("Reached end of stream");
                    StopPlayback();
                }
            }

        }
    }

    private static void OnPlaybackStopped(object sender, StoppedEventArgs e)
    {
        //Debug.WriteLine("Playback Stopped");
        if (e.Exception != null)
        {
            //MessageBox.Show(String.Format("Playback Error {0}", e.Exception.Message));
        }
    }

    public void Play()
    {
        waveOut.Play();
        Console.WriteLine(String.Format("Started playing, waveOut.PlaybackState={0}", waveOut.PlaybackState));
        playbackState = StreamingPlaybackState.Playing;
    }

    private void buttonPlay_Click()
    {
        String s = "";

        if (playbackState == StreamingPlaybackState.Stopped)
        {
            playbackState = StreamingPlaybackState.Buffering;
            bufferedWaveProvider = null;
            ThreadPool.QueueUserWorkItem(StreamAudio);
        }
        else if (playbackState == StreamingPlaybackState.Paused)
        {
            playbackState = StreamingPlaybackState.Buffering;
        }
    }



}

