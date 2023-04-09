using Spine38;
using System;
using System.Collections.Generic;

namespace Pure01fx.SpineConverter
{
    class Worker
    {
        public SkeletonHappy s;
        bool non;

        public Worker(SkeletonHappy s)
        {
            this.s = s;
        }
        public bool Work()
        {
            s.String(); // hash
            var version = s.r.ReadString();
            if (!version.StartsWith("3.8"))
            {
                if (version.StartsWith("3.6"))
                {
                    UnityEngine.Debug.LogFormat("currversion is {0} not need converter",version);
                    return false;
                }
                throw new Exception("input file spine version error version is " + version);
            }
            //s.w.Write("3.8.97"); // version
            s.w.Write("3.6.53");
            UnityEngine.Debug.LogFormat("spine{0}->3.6.53", version);
            s.r.ReadFloat(); //y 3.65没有,只读取
            s.r.ReadFloat(); //y 3.65没有，只读取
            s.Do("ff"); // width, height
            non = s.r.ReadBoolean();
            s.w.Write(non);
            if (non)
            {
                s.Do("fs"); // fps, imgPath
                s.r.ReadString();// audioPath没有，只读
            }
            var ExposedListCount = s.r.ReadInt(true);
            s.Foreach("ExposedList", ExposedListCount, i => {
                s.r.ReadStringToRef();
            });
            s.Foreach("Bones", i =>
            {
                s.String(); // name
                if (i != 0) s.PInt(); // parent
                s.Do("ffff ffff i"); // rotation, x, y, scaleX, scaleY, shearX, shearY, length, transformMode
                s.r.ReadBoolean();//skinRequired没有，只读
                if (non) s.Int(); 
            });
            s.Foreach("Slots", i =>
            {
                s.Do("si"); // name, bone
                s.Int(); // color
                s.Int();//darkColor
                var attachment =  s.r.ReadStringRef();
                s.w.Write(attachment);
                s.Do("i");
            });
            s.Foreach("IKs", i =>
            {
                s.Do("si"); // name, order
                s.r.ReadBoolean(); //skinRequired没有，只读
                s.Foreach(i1 => s.PInt()); // bones
                s.Do("if"); // target, mix
                s.r.ReadFloat();//softness没有，只读
                s.SByte(); // bendDirection
                s.r.ReadBoolean();//compress没有，只读
                s.r.ReadBoolean();//stretch没有，只读
                s.r.ReadBoolean();//uniform没有，只读
            });

            s.Foreach("Transforms", i =>
            {
                s.Do("si"); // name, order
                s.r.ReadBoolean();//skinRequired没有，只读
                s.Foreach(i1 => s.PInt()); // bones
                s.PInt(); // target
                s.Do("00 ffff ffff ff"); // local,relative,offsetRotation, offsetX, offsetY, offsetScaleX, offsetScaleY, offsetShearY, rotateMix, translateMix, scaleMix, shearMix
            });

            s.Foreach("Paths", i =>
            {
                s.Do("si0"); // name, order,skinRequired
                s.r.ReadBoolean();//skinRequired没有，只读
                s.Foreach(i1 => s.PInt()); // bones
                s.Do("iiii ffff f"); // offsetRotation, offsetX, offsetY, offsetScaleX, offsetScaleY, offsetShearY, rotateMix, translateMix, scaleMix, shearMix
            });

            s.Logger("Default skin:");
            s.level += 1;
            ReadSkin(true);
            s.level -= 1;
            s.Foreach("Skins", _ => ReadSkin(false));

            s.Foreach("Events", _ => {
                s.Do("Sifs");
                //下面都是没有的字段，我们只读取
                var AudioPath = s.r.ReadString();
                if(AudioPath != null)
                {
                    s.r.ReadFloat();
                    s.r.ReadFloat();
                }
            });

            s.Foreach("Animations", _ => ReadAnimation());

            //s.w.WriteTmp();
            return true;
        }

        private void ReadAnimation()
        {
            s.String(); // name

            s.Foreach("Slots", _ => {
                s.Log("Slot index", s.PInt());
                s.Foreach(_1 => {
                    var type = s.Byte();
                    s.Foreach((i, n) =>
                    {
                        switch (type)
                        {
                            case 0:
                                s.Do("fS");
                                break;
                            case 1:
                                s.Do("fI");
                                if (i < n - 1) ReadCurve();
                                break;
                            case 2:
                                s.Do("fII");
                                if (i < n - 1) ReadCurve();
                                break;
                            default:
                                throw new Exception();
                        }
                    });
                });
            });

            s.Foreach("Bones", _ => {
                s.Log("Bone index", s.PInt());
                s.Foreach(_1 =>
                {
                    var type = s.Byte();
                    s.Foreach((i, n) =>
                    {
                        switch (type)
                        {
                            case 0: s.Do("ff");
                                break;
                            case 1:
                            case 2:
                            case 3: 
                                s.Do("fff");
                                break;
                            default: throw new Exception();
                        }
                        if (i < n - 1) ReadCurve();
                    });
                });
            });

            s.Foreach("IKs", _ => {
                s.Log("Index", s.PInt());
                s.Foreach((i, n) => {
                    s.Do("ff");
                    s.r.ReadFloat();//没有，只读
                    s.SByte();
                    s.r.ReadBoolean();//没有，只读
                    s.r.ReadBoolean();//没有，只读
                    //s.w.Write(false);
                    //s.w.Write(false); // default: !!!
                    if (i < n - 1) ReadCurve();
                });
            });

            //s.Foreach("Paths", _ => {
            //    s.Log("Index", s.PInt());
            //    s.Foreach((i, n) => {
            //        s.Do("fffff");
            //        if (i < n - 1) ReadCurve();
            //    });
            //});

            s.Foreach("Transforms", _ => {
                s.Log("Index", s.PInt());
                s.Foreach((i, n) => {
                    s.Do("fffff");
                    if (i < n - 1) ReadCurve();
                });
            });

            s.Foreach("Paths", _ => {
                s.Log("Index", s.PInt());
                s.Foreach(_1 =>
                {
                    var type = s.Byte();
                    s.Foreach((i, n) =>
                    {
                        switch (type)
                        {
                            case 0:
                            case 1: s.Do("ff");
                                break;
                            case 2: s.Do("fff");
                                break;
                            default: throw new Exception();
                        }
                        if (i < n - 1) ReadCurve();
                    });
                });
            });

            s.Foreach("Deform", _ =>
            {
                s.Log("Skin", s.PInt());
                s.Foreach(_1 =>
                {
                    s.Log("Slot index", s.PInt());
                    s.Foreach(_2 =>
                    {
                        var attachment = s.r.ReadStringRef();
                        s.w.Write(attachment);
                        s.Foreach((i, n) =>
                        {
                            s.Float(); // time
                            int end = s.PInt();
                            if (end != 0)
                            {
                                s.PInt();
                                s.Foreach(end, _3 => s.Float());
                            }
                            if (i < n - 1) ReadCurve();
                        });
                    });
                });
            });

            s.Foreach("Draworder", _ => {
                s.Float();
                s.Foreach(_1 => s.Do("ii"));
            });


            s.Foreach("Event", _ => {
                s.Float();
                s.PInt();
                s.PInt(false);
                s.Float();
                bool v = s.r.ReadBoolean();
                s.w.Write(v);
                if (v) s.String();
            });
        }

        private void ReadCurve()
        {
            if (s.Byte() == 2) s.Do("ffff");
        }

        private void ReadSkin(bool defaultSkin)
        {
            int slotCount;
            if (defaultSkin)
            {
                slotCount = s.PInt();
            }
            else
            {
                s.String(); // name
                s.r.ReadStringRef();
                //下面都是没有的字段，全部都之读取
                s.Foreach("bones", _ => {
                    s.r.ReadInt(true);
                });
                s.Foreach("ikConstraints", _ => {
                    s.r.ReadInt(true);
                });
                s.Foreach("transformConstraints", _ => {
                    s.r.ReadInt(true);
                });
                s.Foreach("pathConstraints", _ => {
                    s.r.ReadInt(true);
                });
                slotCount = s.PInt();
            }
            s.Foreach("Slots", slotCount, _ =>
            {
                s.Log("Slot index", s.PInt());
                s.Foreach("Attachments", _1 =>
                {
                    var s1 = s.r.ReadStringRef();
                    s.w.Write(s1);
                    ReadAttachment();
                });
            });
        }

        private void ReadAttachment()
        {
            s.Do("R");
            int vertexCount;
            switch (s.Byte())
            {
                case 0: // Region
                    s.Do("R ffff fff");
                    s.Int(); // color
                    break;
                case 1: // Boundingbox
                    ReadVertices(s.PInt());
                    if (non) s.Int();
                    break;
                case 2:
                    s.RefString2String(); // path
                    s.Int(); // color
                    vertexCount = s.PInt();
                    ReadFloatArray(vertexCount << 1);
                    ReadShortArray();
                    ReadVertices(vertexCount);
                    s.PInt();
                    if (non)
                    {
                        ReadShortArray();
                        s.Float();
                        s.Float();
                    }
                    break;
                case 3:
                    s.Do("RiSS0");
                    if (non) s.Do("ff");
                    break;
                case 4:
                    s.Do("00");
                    vertexCount = s.PInt();
                    ReadVertices(vertexCount);
                    for (int i = 0, n = vertexCount / 3; i < n; ++i) s.Float();
                    if (non) s.Int();
                    break;
                case 5:
                    s.Do("fff");
                    if (non)
                        s.Int();
                    break;
                case 6:
                    s.Do("i");
                    vertexCount = s.PInt();
                    ReadVertices(vertexCount);
                    if (non) s.Int();
                    break;
                default:
                    throw new Exception();
            }
        }

        private void ReadVertices(int vertexCount)
        {
            bool v = s.r.ReadBoolean();
            s.w.Write(v);
            if (!v)
            {
                ReadFloatArray(vertexCount << 1);
                return;
            }
            s.Foreach(vertexCount, _ => s.Foreach(_1 => s.Do("ifff")));
        }

        private void ReadFloatArray(int n) => s.Foreach(n, _ => s.Float());

        private void ReadShortArray() => s.Foreach(_ => s.Do("bb"));
    }

    public class SpineConverter
    {
        public static string Main(string FileName)
        {
            //Console.WriteLine("By: xhh->QQ:1094632455");
            SkeletonHappy s;
            Action<bool> dispose;
            s = SkeletonHappy.FromFile(FileName, out dispose);
            try
            {
                bool isSuccess = false;
                if (s._isJson)
                {
                    UnityEngine.Debug.Log(s._isJson);
                    isSuccess = new SkeletonJson36().ReadSkeletonData(FileName, s);
                    if (!isSuccess)
                    {
                        isSuccess = new SkeletonJson38().ReadSkeletonData(FileName, s);
                    }
                }
                else
                {
                    isSuccess = new Worker(s).Work();
                }
                dispose(isSuccess);
                return s.outFileName;
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log(e.ToString());
                dispose(false);
                return null;
            }
            


        }
    }
}
