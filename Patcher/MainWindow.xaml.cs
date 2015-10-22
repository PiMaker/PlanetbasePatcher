namespace Patcher
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;

    using Microsoft.Win32;

    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string GMCTOR = @"    IL_00fe:  call       instance void Planetbase.GameManager::ML_loadMods()
    IL_0103:  ldarg.0
    IL_0104:  call       instance void Planetbase.GameManager::setGameStateLogo()
    IL_0109:  ret";

        private const string GMUPDATE = @"    .maxstack  3
    IL_0000:  ldarg.0
    IL_0001:  ldfld      valuetype Planetbase.GameManager/State Planetbase.GameManager::mState
    IL_0006:  ldc.i4.2
    IL_0007:  bne.un     IL_0032
    IL_000c:  ldarg.0
    IL_000d:  ldfld      class Planetbase.GameState Planetbase.GameManager::mGameState
    IL_0012:  ldarg.1
    IL_0013:  callvirt   instance void Planetbase.GameState::update(float32)
    IL_0018:  ldarg.0
    IL_0019:  call       instance void Planetbase.GameManager::ML_updateMods()
    IL_001e:  call       class Planetbase.CameraManager Planetbase.CameraManager::getInstance()
    IL_0023:  call       float32 [UnityEngine]UnityEngine.Time::get_smoothDeltaTime()
    IL_0028:  callvirt   instance void Planetbase.CameraManager::update(float32)
    IL_002d:  br         IL_003e

    IL_0032:  ldarg.0
    IL_0033:  ldfld      class Planetbase.ScreenRenderer Planetbase.GameManager::mScreenRenderer
    IL_0038:  ldarg.1
    IL_0039:  callvirt   instance void Planetbase.ScreenRenderer::update(float32)
    IL_003e:  ldarg.0
    IL_003f:  ldfld      class Planetbase.GameState Planetbase.GameManager::mGameState
    IL_0044:  callvirt   instance bool Planetbase.GameState::isTitleState()
    IL_0049:  brfalse    IL_0059

    IL_004e:  call       !0 class Planetbase.Singleton`1<class Planetbase.TitleScene>::getInstance()
    IL_0053:  ldarg.1
    IL_0054:  callvirt   instance void Planetbase.TitleScene::update(float32)
    IL_0059:  ldarg.0
    IL_005a:  ldfld      valuetype Planetbase.GameManager/State Planetbase.GameManager::mState
    IL_005f:  ldc.i4.2
    IL_0060:  bne.un     IL_009e

    IL_0065:  ldarg.0
    IL_0066:  ldfld      float32 Planetbase.GameManager::mFadeInTimer
    IL_006b:  ldc.r4     0.0
    IL_0070:  ble.un     IL_009e

    IL_0075:  ldarg.0
    IL_0076:  dup
    IL_0077:  ldfld      float32 Planetbase.GameManager::mFadeInTimer
    IL_007c:  ldarg.1
    IL_007d:  sub
    IL_007e:  stfld      float32 Planetbase.GameManager::mFadeInTimer
    IL_0083:  ldarg.0
    IL_0084:  ldfld      float32 Planetbase.GameManager::mFadeInTimer
    IL_0089:  ldc.r4     0.0
    IL_008e:  bge.un     IL_009e

    IL_0093:  ldarg.0
    IL_0094:  ldc.r4     0.0
    IL_0099:  stfld      float32 Planetbase.GameManager::mFadeInTimer
    IL_009e:  ldarg.0
    IL_009f:  dup
    IL_00a0:  ldfld      int32 Planetbase.GameManager::mCurrentFrameCount
    IL_00a5:  ldc.i4.1
    IL_00a6:  add
    IL_00a7:  stfld      int32 Planetbase.GameManager::mCurrentFrameCount
    IL_00ac:  ldarg.0
    IL_00ad:  dup
    IL_00ae:  ldfld      float32 Planetbase.GameManager::mTimer
    IL_00b3:  ldarg.1
    IL_00b4:  add
    IL_00b5:  stfld      float32 Planetbase.GameManager::mTimer
    IL_00ba:  ldarg.0
    IL_00bb:  ldfld      float32 Planetbase.GameManager::mTimer
    IL_00c0:  ldc.r4     1.
    IL_00c5:  blt.un     IL_00ef

    IL_00ca:  ldarg.0
    IL_00cb:  ldarg.0
    IL_00cc:  ldfld      int32 Planetbase.GameManager::mCurrentFrameCount
    IL_00d1:  stfld      int32 Planetbase.GameManager::mLastFrameCount
    IL_00d6:  ldarg.0
    IL_00d7:  ldc.i4.0
    IL_00d8:  stfld      int32 Planetbase.GameManager::mCurrentFrameCount
    IL_00dd:  ldarg.0
    IL_00de:  dup
    IL_00df:  ldfld      float32 Planetbase.GameManager::mTimer
    IL_00e4:  ldc.r4     1.
    IL_00e9:  sub
    IL_00ea:  stfld      float32 Planetbase.GameManager::mTimer
    IL_00ef:  ret
  }";

        public const string MLFUNCS = @"  .method public hidebysig
    instance void ML_loadMods () cil managed 
  {
	// Method begins at RVA 0x30514
	// Code size 179 (0xb3)
	.maxstack 3
	.locals init (
		[0] class [mscorlib]System.Collections.Generic.Dictionary`2<object, class [mscorlib]System.Reflection.MethodInfo>,
		[1] string,
		[2] string[],
		[3] int32,
		[4] string,
		[5] string,
		[6] class [mscorlib]System.Reflection.Assembly,
		[7] class [mscorlib]System.Type,
		[8] object,
		[9] class [mscorlib]System.Reflection.MethodInfo
	)

	IL_0000: newobj instance void class [mscorlib]System.Collections.Generic.Dictionary`2<object, class [mscorlib]System.Reflection.MethodInfo>::.ctor()
	IL_0005: stloc.0
	IL_0006: call string ['Assembly-CSharp']Planetbase.Util::getFilesFolder()
	IL_000b: ldsfld char [mscorlib]System.IO.Path::DirectorySeparatorChar
	IL_0010: box [mscorlib]System.Char
	IL_0015: ldstr " + "\"Mods\"" + @"
	IL_001a: call string[mscorlib] System.String::Concat(object, object, object)
    IL_001f: stloc.1
	IL_0020: ldloc.1
	IL_0021: call bool[mscorlib] System.IO.Directory::Exists(string)
    IL_0026: brfalse IL_00ac

    IL_002b: ldloc.1
	IL_002c: ldstr " + "\"*.dll\"" + @"
	IL_0031: call string[][mscorlib] System.IO.Directory::GetFiles(string, string)
    IL_0036: stloc.2
	IL_0037: ldc.i4.0
	IL_0038: stloc.3
	IL_0039: br.s IL_00a6
    // loop start (head: IL_00a6)
        IL_003b: ldloc.2
		IL_003c: ldloc.3
		IL_003d: ldelem.ref
		IL_003e: stloc.s 4
		IL_0040: ldloc.s 4
		IL_0042: call string[mscorlib] System.IO.Path::GetFileNameWithoutExtension(string)
        IL_0047: stloc.s 5
		IL_0049: ldloc.s 4
		IL_004b: call uint8[] [mscorlib]System.IO.File::ReadAllBytes(string)
		IL_0050: call class [mscorlib]System.Reflection.Assembly[mscorlib] System.Reflection.Assembly::Load(uint8[])
        IL_0055: stloc.s 6
		IL_0057: ldloc.s 6
		IL_0059: ldloc.s 5
		IL_005b: ldstr " + "\".Mod\"" + @"
		IL_0060: call string[mscorlib] System.String::Concat(string, string)
        IL_0065: callvirt instance class [mscorlib]System.Type[mscorlib] System.Reflection.Assembly::GetType(string)
        IL_006a: stloc.s 7
		IL_006c: ldloc.s 7
		IL_006e: call object[mscorlib] System.Activator::CreateInstance(class [mscorlib]System.Type)
		IL_0073: stloc.s 8
		IL_0075: ldloc.s 7
		IL_0077: ldstr " + "\"Init\"" + @"
		IL_007c: callvirt instance class [mscorlib]System.Reflection.MethodInfo[mscorlib] System.Type::GetMethod(string)
        IL_0081: ldloc.s 8
		IL_0083: ldnull
        IL_0084: callvirt instance object[mscorlib] System.Reflection.MethodBase::Invoke(object, object[])
        IL_0089: pop
        IL_008a: ldloc.s 7
		IL_008c: ldstr " + "\"Update\"" + @"
		IL_0091: callvirt instance class [mscorlib]System.Reflection.MethodInfo[mscorlib] System.Type::GetMethod(string)
        IL_0096: stloc.s 9
		IL_0098: ldloc.0
		IL_0099: ldloc.s 8
		IL_009b: ldloc.s 9
		IL_009d: callvirt instance void class [mscorlib]System.Collections.Generic.Dictionary`2<object, class [mscorlib]System.Reflection.MethodInfo>::Add(!0, !1)
        IL_00a2: ldloc.3
		IL_00a3: ldc.i4.1
		IL_00a4: add
        IL_00a5: stloc.3

		IL_00a6: ldloc.3
		IL_00a7: ldloc.2
		IL_00a8: ldlen
        IL_00a9: conv.i4
        IL_00aa: blt.s IL_003b
    // end loop

    IL_00ac: ldloc.0
	IL_00ad: stsfld object Planetbase.GameManager::ML_modList
    IL_00b2: ret
  }

  .method public hidebysig instance void
          ML_updateMods() cil managed
  {
    // Code size       75 (0x4b)
    .maxstack  3
    .locals init (class [mscorlib] System.Collections.Generic.Dictionary`2<object,class [mscorlib] System.Reflection.MethodInfo> V_0,valuetype[mscorlib] System.Collections.Generic.KeyValuePair`2<object,class [mscorlib] System.Reflection.MethodInfo> V_1,valuetype[mscorlib] System.Collections.Generic.Dictionary`2/Enumerator<object,class [mscorlib] System.Reflection.MethodInfo> V_2)
    IL_0000:  ldsfld object Planetbase.GameManager::ML_modList
    IL_0005:  castclass  class [mscorlib] System.Collections.Generic.Dictionary`2<object,class [mscorlib] System.Reflection.MethodInfo>
    IL_000a:  stloc.0
    IL_000b:  ldloc.0
    IL_000c:  callvirt instance valuetype[mscorlib] System.Collections.Generic.Dictionary`2/Enumerator<!0,!1> class [mscorlib] System.Collections.Generic.Dictionary`2<object,class [mscorlib] System.Reflection.MethodInfo>::GetEnumerator()
    IL_0011:  stloc.2
    .try
    {
      IL_0012:  br.s IL_0031

      IL_0014:  ldloca.s V_2
      IL_0016:  call instance valuetype[mscorlib] System.Collections.Generic.KeyValuePair`2<!0,!1> valuetype[mscorlib] System.Collections.Generic.Dictionary`2/Enumerator<object,class [mscorlib] System.Reflection.MethodInfo>::get_Current()
      IL_001b:  stloc.1
      IL_001c:  ldloca.s V_1
      IL_001e:  call instance !1 valuetype[mscorlib] System.Collections.Generic.KeyValuePair`2<object,class [mscorlib] System.Reflection.MethodInfo>::get_Value()
      IL_0023:  ldloca.s V_1
      IL_0025:  call instance !0 valuetype[mscorlib] System.Collections.Generic.KeyValuePair`2<object,class [mscorlib] System.Reflection.MethodInfo>::get_Key()
      IL_002a:  ldnull
      IL_002b:  callvirt instance object[mscorlib] System.Reflection.MethodBase::Invoke(object,object[])
      IL_0030:  pop
      IL_0031:  ldloca.s V_2
      IL_0033:  call instance bool valuetype[mscorlib]System.Collections.Generic.Dictionary`2/Enumerator<object,class [mscorlib] System.Reflection.MethodInfo>::MoveNext()
      IL_0038:  brtrue.s IL_0014

      IL_003a:  leave.s IL_004a

    }  // end .try
    finally
    {
      IL_003c:  ldloca.s V_2
      IL_003e:  constrained. valuetype[mscorlib] System.Collections.Generic.Dictionary`2/Enumerator<object,class [mscorlib] System.Reflection.MethodInfo>
      IL_0044:  callvirt instance void[mscorlib] System.IDisposable::Dispose()
      IL_0049:  endfinally
    }  // end handler
    IL_004a:  ret
  }";

        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void FrameworkElement_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Width = this.buttonSelect.ActualWidth + 30 + this.labelDll.ActualWidth
                         + (string.IsNullOrEmpty(this.labelDll.Text) ? 5 : 10);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog
                          {
                              Filter = "Assembly-CSharp.dll|Assembly-CSharp.dll",
                              Multiselect = false,
                              Title = "Choose Assembly-CSharp.dll location"
                          };
            if (ofd.ShowDialog().GetValueOrDefault(false))
            {
                this.labelDll.Text = ofd.FileName;
                this.FrameworkElement_OnSizeChanged(this, null);
                this.buttonPatch.IsEnabled = true;
            }
        }

        private async void ButtonBase2_OnClick(object sender, RoutedEventArgs e)
        {
            // Do the stuff
            if (string.IsNullOrEmpty(this.labelDll.Text))
            {
                return;
            }
            if (!File.Exists(this.labelDll.Text))
            {
                return;
            }

            this.buttonSelect.IsEnabled = false;
            this.buttonPatch.IsEnabled = false;

            this.buttonPatch.Content = "Copying working DLL...";

            await this.labelDll.Dispatcher.InvokeAsync(
                () =>
                    {
                        if (File.Exists("Assembly-CSharp.dll"))
                        {
                            File.Delete("Assembly-CSharp.dll");
                        }

                        File.Copy(this.labelDll.Text, "Assembly-CSharp.dll");
                    });

            this.buttonPatch.Content = "Preparing ILDASM...";

            if (!File.Exists("ildasm.exe"))
            {
                using (
                    var ildasm = Application.GetResourceStream(new Uri("ildasm.exe", UriKind.RelativeOrAbsolute)).Stream
                    )
                using (var ildasmfile = new FileStream("ildasm.exe", FileMode.Create))
                {
                    await ildasm.CopyToAsync(ildasmfile);
                }
            }

            if (File.Exists("Assembly-CSharp.il"))
            {
                File.Delete("Assembly-CSharp.il");
            }

            if (File.Exists("Assembly-CSharp.res"))
            {
                File.Delete("Assembly-CSharp.res");
            }

            if (File.Exists("Assembly-CSharp-orig.il"))
            {
                File.Delete("Assembly-CSharp-orig.il");
            }

            this.buttonPatch.Content = "Decompiling...";

            var ildasmProc = Process.Start("ildasm.exe", "/out=Assembly-CSharp.il /utf8 Assembly-CSharp.dll");
            await Task.Run(() => ildasmProc.WaitForExit());

            File.Copy("Assembly-CSharp.il", "Assembly-CSharp-orig.il");

            this.buttonPatch.Content = "Injecting IL...";

            var inGameManagerClass = false;

            using (var ILStream = new FileStream("Assembly-CSharp-orig.il", FileMode.Open))
            using (var reader = new StreamReader(ILStream, Encoding.UTF8))
            using (var ILStreamNew = new FileStream("Assembly-CSharp.il", FileMode.Open))
            using (var writer = new StreamWriter(ILStreamNew, Encoding.UTF8))
            {
                ILStream.Seek(0, SeekOrigin.Begin);
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();

                    if (line.Contains(".class public auto ansi beforefieldinit Planetbase.GameManager"))
                    {
                        inGameManagerClass = true;
                        goto ContinueWithWrite;
                    }

                    if (line.Contains(".class") && !line.Contains("nested"))
                    {
                        inGameManagerClass = false;
                        goto ContinueWithWrite;
                    }

                    if (inGameManagerClass && line.Contains(".method public hidebysig instance void"))
                    {
                        await writer.WriteLineAsync(line);
                        line = await reader.ReadLineAsync();
                        if (line.Contains("update(float32 timeStep) cil managed"))
                        {
                            await writer.WriteLineAsync(line);
                            line = reader.ReadLine();
                            if (line.Contains("{"))
                            {
                                await writer.WriteLineAsync(line);
                                await writer.WriteLineAsync(MainWindow.GMUPDATE);
                                while (!(await reader.ReadLineAsync()).Contains("}"))
                                {
                                }
                                continue;
                            }
                        }
                    }

                    if (inGameManagerClass && line.Contains(".method public hidebysig specialname rtspecialname"))
                    {
                        var templine = await reader.ReadLineAsync();
                        if (templine.Contains("instance void  .ctor() cil managed"))
                        {
                            await writer.WriteLineAsync("  .field public static object ML_modList");
                            await writer.WriteLineAsync(line);
                            await writer.WriteLineAsync(templine);
                            continue;
                        }
                        await writer.WriteLineAsync(line);
                        await writer.WriteLineAsync(templine);
                        continue;
                    }

                    if (inGameManagerClass
                        && line.Contains("call       instance void Planetbase.GameManager::setGameStateLogo()"))
                    {
                        var templine = await reader.ReadLineAsync();
                        if (templine.Contains("ret"))
                        {
                            await writer.WriteLineAsync(MainWindow.GMCTOR);
                            continue;
                        }
                        await writer.WriteLineAsync(line);
                        await writer.WriteLineAsync(templine);
                        continue;
                    }

                    if (inGameManagerClass && line.Contains("} // end of class Planetbase.GameManager"))
                    {
                        await writer.WriteLineAsync(MainWindow.MLFUNCS);
                    }

                    ContinueWithWrite:
                    await writer.WriteLineAsync(line);
                }
            }

            this.buttonPatch.Content = "Preparing ILASM...";

            if (!File.Exists("ilasm.exe"))
            {
                using (
                    var ildasm = Application.GetResourceStream(new Uri("ilasm.exe", UriKind.RelativeOrAbsolute)).Stream)
                using (var ildasmfile = new FileStream("ilasm.exe", FileMode.Create))
                {
                    await ildasm.CopyToAsync(ildasmfile);
                }
            }

            if (!File.Exists("fusion.dll"))
            {
                using (
                    var fusion = Application.GetResourceStream(new Uri("fusion.dll", UriKind.RelativeOrAbsolute)).Stream
                    )
                using (var fusionfile = new FileStream("fusion.dll", FileMode.Create))
                {
                    await fusion.CopyToAsync(fusionfile);
                }
            }

            File.Delete("Assembly-CSharp.dll");

            this.buttonPatch.Content = "Recompiling...";

            var ilasmProc = Process.Start("ilasm.exe", "/dll Assembly-CSharp.il");
            await Task.Run(() => ilasmProc.WaitForExit());

            this.buttonPatch.Content = "Backing up...";

            var bckPath = Path.Combine(Path.GetDirectoryName(this.labelDll.Text), "Assembly-CSharp.dll.bck");
            if (!File.Exists(bckPath))
            {
                await this.labelDll.Dispatcher.InvokeAsync(() => { File.Copy(this.labelDll.Text, bckPath); });
            }

            this.buttonPatch.Content = "Installing ML...";

            await this.labelDll.Dispatcher.InvokeAsync(
                () =>
                    {
                        if (File.Exists(this.labelDll.Text))
                        {
                            File.Delete(this.labelDll.Text);
                        }

                        File.Move("Assembly-CSharp.dll", this.labelDll.Text);
                    });

            this.buttonPatch.Content = "Creating Mods-Folder...";

            var modsFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "Planetbase",
                "Mods");
            if (!Directory.Exists(modsFolder))
            {
                Directory.CreateDirectory(modsFolder);
            }

            this.buttonPatch.Content = "Cleaning up...";

            await Task.Run(
                () =>
                    {
                        File.Delete("Assembly-CSharp.il");
                        File.Delete("Assembly-CSharp-orig.il");
                        File.Delete("Assembly-CSharp.res");
                        File.Delete("fusion.dll");
                        File.Delete("ilasm.exe");
                        File.Delete("ildasm.exe");
                    });

            this.buttonPatch.Content = "Done!";
        }
    }
}