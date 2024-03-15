using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace DHCPdisable
{
    public class ProcessStart
    {
        // DHCP 클라이언트 서비스 중지
        public static bool stopDHCPClientService()
        {
            string fileName = Environment.GetFolderPath(Environment.SpecialFolder.System);
            fileName += "\\cmd.exe";
            bool b = processStart(fileName, "/C net stop dhcp","y\n");
            if (b) Console.WriteLine("DHCP 클라이언트 서비스가 중지되었습니다.");
            return b;
        }

        // APIPA 비활성화
        public static bool disableAPIPA()
        {
            string fileName = Environment.GetFolderPath(Environment.SpecialFolder.System);
            fileName += "\\cmd.exe";
            bool b = processStart(fileName, "/C netsh interface ipv4 set address \"Local Area Connection\" dhcp");
            if (b) Console.WriteLine("APIPA가 비활성화되었습니다.");
            return b;
        }

        // IP주소 갱신
        public static bool renewIPAddress()
        {
            string fileName = Environment.GetFolderPath(Environment.SpecialFolder.System);
            fileName += "\\ipconfig.exe";
            bool b = processStart(fileName, "/renew");
            if (b) Console.WriteLine("IP 주소가 갱신되었습니다.");
            return b;
        }

        private static bool processStart(string fileName, string arguments , string textInput="")
        {
            if (!File.Exists(fileName))
            {
                Console.WriteLine("실행 파일 이름이 비어 있습니다.");
                return false;
            }

            ProcessStartInfo startInfo = new ProcessStartInfo(fileName, arguments)
            {
                UseShellExecute = false, // 셸을 사용하지 않음
                CreateNoWindow = true, // 새 창을 열지 않음
                RedirectStandardInput = true, // 표준 입력 리다이렉트
                RedirectStandardOutput = true, // 필요한 경우 표준 출력도 리다이렉트
                RedirectStandardError = true // 필요한 경우 표준 오류도 리다이렉트
            };

            try
            {
                using (Process process = Process.Start(startInfo))
                {
                    if (!string.IsNullOrEmpty(textInput))
                    {
                        using (StreamWriter sw = process.StandardInput)
                        {
                            if (sw.BaseStream.CanWrite)
                            {
                                sw.WriteLine(textInput); // 표준 입력을 통해 textInput 내용을 전송
                            }
                        }
                    }

                    process.WaitForExit(); // 프로세스 실행 완료 대기
                    return true;
                }
            }
            catch (System.ObjectDisposedException ex)
            {
                Console.WriteLine("The process object has already been disposed. [" + ex.Message + "]");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("The fileName or arguments parameter is null. [" + ex.Message + "]");
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                Console.WriteLine("해당 프로그램을 엑세스 할 수 없습니다.[파일경로길이가 2080 이상일 경우 문제가 발생할 수 있습니다.] [" + ex.Message + "]");
            }
            catch (System.IO.FileNotFoundException ex)
            {
                Console.WriteLine("The PATH environment variable has a string containing quotes. [" + ex.Message + "]");
            }
            catch (Exception ex)
            {
                Console.WriteLine("알수없는 에러 [" + ex.Message + "]");
            }

            return false;
        }
    }
}