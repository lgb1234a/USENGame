using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure01fx.SpineConverter
{
    public class SkeletonHappy
    {
        public SkeletonInput r = null;
        public SkeletonOutput w = null;
        public bool _isJson = false;
        public string fileName;
        public string outFileName;
        public SkeletonHappy(Stream input, Stream output,bool isJson = false,string name = "",string outName = "")
        {
            _isJson = isJson;
            fileName = name;
            outFileName = outName;
            Logger = s => {
                for (var i = 0; i < level; ++i) Console.Write('\t');
                Console.WriteLine(s);
            };
            r = new SkeletonInput(input, Logger);
            w = new SkeletonOutput(output);
        }
        
        public int level;
        public Action<string> Logger { get; }

        public void Log(string k, object v) => Logger($"{k} = {v}");

        public static SkeletonHappy FromFile(string file, out Action<bool> ReleaseStream)
        {
            var inputName = file;
            if (!file.EndsWith(".skel.bytes") && !file.EndsWith(".json"))
            {
                throw new Exception("Filename should ends with .skel.bytes or json");
            }
            var outputName = file.Substring(0,file.Length - 11) + ".skel_new.bytes";
            var isJson = file.EndsWith(".json");
            if (isJson)
            {
                outputName = file.Substring(0, file.Length - 5) + ".skel.bytes";
            }
            
            FileStream input = null;
            if(!isJson)
                input = File.OpenRead(inputName);
            var output = File.OpenWrite(outputName);
            ReleaseStream = (isSuccess) =>
            {
                if (!isJson && input != null)
                    input.Dispose();
                output.Dispose();
                UnityEngine.Debug.Log(isSuccess);
                if (!isSuccess)
                {
                    if (File.Exists(outputName))
                    {
                        File.Delete(outputName);
                        UnityEngine.Debug.Log("Delete:" + outputName);
                    }
                    
                }
                else
                {
					
					if (!isJson)
                    {
                        if (File.Exists(inputName))
                            File.Delete(inputName);
                        if (File.Exists(outputName))
                            File.Move(outputName, inputName);
                    }
                    
                }
                //if (File.Exists(outputName))
                //{
                //    FileInfo fi2 = new FileInfo(outputName);
                //    fi2.CopyTo(file, true);
                //    File.Delete(outputName);
                //}
                
            };
            return new SkeletonHappy(input, output, isJson, inputName);
        }

        public int Int() { var v = r.ReadInt(); w.WriteInt32(v); return v; }

        public int PInt(bool optimizePositive = true)
        {
            int b = Byte();
            int result = b & 0x7F;
            if ((b & 0x80) != 0)
            {
                b = Byte();
                result |= (b & 0x7F) << 7;
                if ((b & 0x80) != 0)
                {
                    b = Byte();
                    result |= (b & 0x7F) << 14;
                    if ((b & 0x80) != 0)
                    {
                        b = Byte();
                        result |= (b & 0x7F) << 21;
                        if ((b & 0x80) != 0) result |= (Byte() & 0x7F) << 28;
                    }
                }
            }
            return optimizePositive ? result : ((result >> 1) ^ -(result & 1));
        }


        public int PIntNew(bool optimizePositive = true)
        {
            int b = ByteNew();
            int result = b & 0x7F;
            if ((b & 0x80) != 0)
            {
                b = ByteNew();
                result |= (b & 0x7F) << 7;
                if ((b & 0x80) != 0)
                {
                    b = ByteNew();
                    result |= (b & 0x7F) << 14;
                    if ((b & 0x80) != 0)
                    {
                        b = ByteNew();
                        result |= (b & 0x7F) << 21;
                        if ((b & 0x80) != 0) result |= (ByteNew() & 0x7F) << 28;
                    }
                }
            }
            return optimizePositive ? result : ((result >> 1) ^ -(result & 1));
        }

        public void Float()
        {
            Byte();
            Byte();
            Byte();
            Byte();
        }

        public void Float(int repeatTime)
        {
            for (int i = 0; i < repeatTime; ++i) Float();
        }


        public void StringNew()
        {
            int byteCount = PIntNew(true);
            switch (byteCount)
            {
                case 0:
                case 1:
                    return;
            }
            byteCount--;
            byte[] buffer = new byte[byteCount];
            r.ReadFully(buffer, 0, byteCount);
            w.Write(buffer);
            Logger(Encoding.UTF8.GetString(buffer, 0, byteCount));
        }

        public void String()
        {
            int byteCount = PInt(true);
            switch (byteCount)
            {
                case 0:
                case 1:
                    return;
            }
            byteCount--;
            byte[] buffer = new byte[byteCount];
            r.ReadFully(buffer, 0, byteCount);
            w.Write(buffer);
            Logger(Encoding.UTF8.GetString(buffer, 0, byteCount));
        }

        public void RefString2String()
        {
            w.Write(r.ReadStringRef());
        }
        public void Bool() => w.Write(r.ReadBoolean());

        public void SByte() => w.Write(r.ReadSByte());

        public byte Byte() { var v = r.ReadByte(); w.Write(v); return v; }
        public byte ByteNew() { var v = r.ReadByte(); return v; }

        public void RefString() => w.WriteStringRef(r.ReadString());

        public void Do(string cmd, params Action[] actions)
        {
            int current = 0;
            foreach (var i in cmd)
            {
                switch (i)
                {
                    case 's':
                        String();
                        break;
                    case 'S':
                        //RefString();
                        RefString2String();
                        //String();
                        break;
                    case 'R':
                        RefString2String();
                        break;
                    case 'i':
                        PInt();
                        break;
                    case 'I':
                        Int();
                        break;
                    case 'f':
                        Float();
                        break;
                    case 'b':
                        Byte();
                        break;
                    case '0':
                        Bool();
                        break;
                    case '_':
                        actions[current]();
                        ++current;
                        break;
                }
            }
        }

        public void Foreach(string name, Action<int> action)
        {
            Foreach(name, PInt(), (i, _) => action(i));
        }

        public void Foreach(string name, Action<int, int> action)
        {
            Foreach(name, PInt(), action);
        }

        public void Foreach(Action<int> action)
        {
            Foreach(PInt(), action);
        }

        public void Foreach(Action<int, int> action)
        {
            Foreach(PInt(), action);
        }

        public void Foreach(string name, int n, Action<int> action)
        {
            Foreach(name, n, (i, _) => action(i));
        }

        public void Foreach(string name, int n, Action<int, int> action)
        {
            Logger($"{name}({n}): ");
            level += 1;
            for (int i = 0; i < n; ++i)
            {
                Logger($"[{i}]");
                action(i, n);
            }
            level -= 1;
        }

        public void Foreach(int n, Action<int> action)
        {
            Foreach(n, (i, _) => action(i));
        }

        public void Foreach(int n, Action<int, int> action)
        {
            level += 1;
            for (int i = 0; i < n; ++i)
            {
                action(i, n);
            }
            level -= 1;
        }
    }

    public class SkeletonOutput
    {
        private byte[] chars = Array.Empty<byte>();
        internal List<string> strings = new List<string>();
        private Dictionary<string, int> refId = new Dictionary<string, int>();
        public Stream output;
        Stream real, tmp;

        public SkeletonOutput(Stream stream)
        {
            real = output = stream;
            tmp = new MemoryStream();
        }

        public void SwitchToTempStream()
        {
            output = tmp;
        }

        public void WriteTmp()
        {
            output = real;
            //if (strings.Count > 0x7f) throw new Exception("string ref array is too large");
            WriteOptimizedPositiveInt(strings.Count);
            foreach (var i in strings) Write(i);
            tmp.Position = 0;
            tmp.CopyTo(real);
        }

        public void WriteStringRef(string str,bool isNeedWrite = true)
        {
            if (str == null)
            {
                Write((byte)0);
                return;
            }
            if (refId.ContainsKey(str) == false)
            {
                strings.Add(str);
                refId[str] = strings.Count;
            }
            //if (refId[str] > 0x7f) throw new Exception("Id too large");
            //Write((byte)refId[str]);
            WriteOptimizedPositiveInt(refId[str]);
        }

        public void Write(byte val) => output.WriteByte(val);
        public void Write(sbyte val) => output.WriteByte(unchecked((byte)val));//TODO:
        public void Write(bool val) => output.WriteByte(val ? byte.MaxValue : byte.MinValue);//TODO:
        public void Write(float val)
        {
            chars = BitConverter.GetBytes(val);
            byte tmp = chars[3];
            chars[3] = chars[0];
            chars[0] = tmp;
            tmp = chars[2];
            chars[2] = chars[1];
            chars[1] = tmp;
            output.Write(chars, 0, chars.Length);
        }
        [Obsolete("Use the direct WriteOptimizedPositiveInt or WriteInt32")]
        public void Write(int val)
        {
            WriteInt32(val);
        }
        public void WriteInt32(int val)
        {
            Write((byte)(val >> 24));
            Write((byte)(val >> 16));
            Write((byte)(val >> 8));
            Write((byte)val);
        }
        public void Write(byte r, byte g, byte b)
        {
            Write((byte)0);
            Write(r);
            Write(g);
            Write(b);
        }
        public void Write(byte r, byte g, byte b, byte a)
        {
            Write(r);
            Write(g);
            Write(b);
            Write(a);
        }
        public void Write(string cmd, params object[] args)
        {
            int current = 0;
            foreach (var i in cmd)
            {
                switch (i)
                {
                    case 's':
                        Write((string)args[current]); current++;
                        break;
                    case 'S':
                        WriteStringRef((string)args[current]); ; current++;
                        break;
                    case 'R':
                        break;
                    case 'i':
                        WriteInt32((int)args[current]); ; current++;
                        break;
                    case 'I':
                        WriteInt32((int)args[current]); ; current++;
                        break;
                    case 'p':
                        WriteOptimizedPositiveInt((int)args[current]); ; current++;
                        break;
                    case 'f':
                        Write((float)args[current]); ; current++;
                        break;
                    case 'b':
                        Write((byte)args[current]); ; current++;
                        break;
                    case 'B':
                        int tmp = (int)args[current];
                        Write((sbyte)tmp);
                        current++;
                        break;
                    case '0':
                        Write((bool)args[current]); ; current++;
                        break;
                    case '_':
                        current++;
                        break;
                }
            }
        }
        public void Write(string str)
        {
            if (str == null)
            {
                Write((byte)0);
                return;
            }
            if (str == "")
            {
                Write((byte)1);
                return;
            }
            if (str.Length >= 0x7f) throw new Exception("String too long");
            Write((byte)(str.Length + 1));
            chars = Encoding.UTF8.GetBytes(str);
            output.Write(chars, 0, chars.Length);
        }

        public void WriteOptimizedPositiveInt(int val)
        {
            while (true)
            {
                byte wait = (byte)(val & 0x7f);
                if (wait != val) wait |= 0x80;
                Write(wait);
                if (val < 0x80) return;
                val >>= 7;
            }
        }

        internal void Write(byte[] buffer)
        {
            output.Write(buffer, 0, buffer.Length);
        }
    }

    public class SkeletonInput
    {
        private byte[] chars = new byte[32];
        readonly List<string> strings = new List<string>();
        readonly Stream input;
        readonly Action<string> logger;

        public SkeletonInput(Stream input, Action<string> logger)
        {
            this.input = input;
            this.logger = logger;
        }

        public byte ReadByte()
        {
            return (byte)input.ReadByte();
        }

        public sbyte ReadSByte()
        {
            int value = input.ReadByte();
            if (value == -1) throw new EndOfStreamException();
            return (sbyte)value;
        }

        public bool ReadBoolean()
        {
            return input.ReadByte() != 0;
        }

        public float ReadFloat()
        {
            chars[3] = (byte)input.ReadByte();
            chars[2] = (byte)input.ReadByte();
            chars[1] = (byte)input.ReadByte();
            chars[0] = (byte)input.ReadByte();
            return BitConverter.ToSingle(chars, 0);
        }

        public int ReadInt()
        {
            return (input.ReadByte() << 24) + (input.ReadByte() << 16) + (input.ReadByte() << 8) + input.ReadByte();
        }

        public int ReadInt(bool optimizePositive)
        {
            int b = input.ReadByte();
            int result = b & 0x7F;
            if ((b & 0x80) != 0)
            {
                b = input.ReadByte();
                result |= (b & 0x7F) << 7;
                if ((b & 0x80) != 0)
                {
                    b = input.ReadByte();
                    result |= (b & 0x7F) << 14;
                    if ((b & 0x80) != 0)
                    {
                        b = input.ReadByte();
                        result |= (b & 0x7F) << 21;
                        if ((b & 0x80) != 0) result |= (input.ReadByte() & 0x7F) << 28;
                    }
                }
            }
            return optimizePositive ? result : ((result >> 1) ^ -(result & 1));
        }

        public string ReadString()
        {
            int byteCount = ReadInt(true);
            switch (byteCount)
            {
                case 0:
                    return null;
                case 1:
                    return "";
            }
            byteCount--;
            byte[] buffer = this.chars;
            if (buffer.Length < byteCount) buffer = new byte[byteCount];
            ReadFully(buffer, 0, byteCount);
            logger(Encoding.UTF8.GetString(buffer, 0, byteCount));
            return Encoding.UTF8.GetString(buffer, 0, byteCount);
        }

        ///<return>May be null.</return>
        public string ReadStringRef()
        {
            int index = ReadInt(true);
            return index == 0 ? null : strings[index - 1];
        }

        public string ReadStringToRef()
        {
            var str = ReadString();
            strings.Add(str);
            return str;
        }

        public void ReadFully(byte[] buffer, int offset, int length)
        {
            while (length > 0)
            {
                int count = input.Read(buffer, offset, length);
                if (count <= 0) throw new EndOfStreamException();
                offset += count;
                length -= count;
            }
        }

        /// <summary>Returns the version string of binary skeleton data.</summary>
        public string GetVersionString()
        {
            try
            {
                // Hash.
                int byteCount = ReadInt(true);
                if (byteCount > 1) input.Position += byteCount - 1;

                // Version.
                byteCount = ReadInt(true);
                if (byteCount > 1)
                {
                    byteCount--;
                    var buffer = new byte[byteCount];
                    ReadFully(buffer, 0, byteCount);
                    return System.Text.Encoding.UTF8.GetString(buffer, 0, byteCount);
                }

                throw new ArgumentException("Stream does not contain a valid binary Skeleton Data.", "input");
            }
            catch (Exception e)
            {
                throw new ArgumentException("Stream does not contain a valid binary Skeleton Data.\n" + e, "input");
            }
        }
    }

    class StreamHelper
    {
    }
}
