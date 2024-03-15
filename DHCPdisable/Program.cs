using System;
using System.Threading;


namespace DHCPdisable
{
    class Program
    {
        static void Main(string[] args)
        {
            bool b = false;
            b = ProcessStart.stopDHCPClientService();               // 1. DHCP 서비스 중지
            if (b) b = ProcessStart.disableAPIPA();                             // 2. APIPA 비활성화
            if (b) b = ProcessStart.renewIPAddress();                         // 3. IP주소 갱신
            Thread.Sleep(1000);                                         // 슬립 1초

            string msg = "\r\n작업이 완료 되었습니다.";
            if (!b) msg = "\r\n작업이 실패 되었습니다.";

            msg += "\r\n아무키나 눌러 프로그램을 종료해주세요";
            Console.WriteLine(msg);
            Console.ReadLine();                                         // 이걸 넣어야 프로그램이 바로 안꺼진다고 한다
        }
    }
}
