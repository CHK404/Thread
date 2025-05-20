using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

//ans
namespace WindowsFormsApp_17_Thread
{
    public partial class Form3 : Form
    {
        //1. 기본 상수 및 상태 변수 정의
        const int FINISH_LINE = 100;    //const: 변하지 않는 상수  //도착 지점 상수 선언
        int rank = 1;   //도착 순위
        int finishedCars = 0;   //도착한 차량 수
        List<Thread> threads = new List<Thread>();  //각 차량의 스레드 저장
        string[] cars = { "A", "B", "C", "D", "E" };

        Random rand = new Random(); //Random 객체 생성(전역 1개만)

        //10. Lock 객체 생성
        static object lockObject = new object();    //공유 데이터 보호용
        public Form3()
        {
            InitializeComponent();

            //9. 스레드 실행(레이스 시작)
            StartRace();
        }
        //8. 차량마다 스레드를 생성해서 동시 실행
        void StartRace()
        {
            foreach (string name in cars)
            {
                Thread t = new Thread(() => RunRace(name));
                threads.Add(t);
                t.Start();  //스레드 실행
            }
        }
        //2. 각 차량이 실행되는 레이스 로직
        void RunRace(string car)
        {
            int distance = 0;   //출발 지점의 거리(위치)를 담는 변수
            int waitTime;   //지연시간을 담는 변수   //레이스 변별력 만들기용?

            //3. 레이스 시작 시간 기록
            DateTime startTime = DateTime.Now;  //Now를 이용해 현재 시간 startTime에 할당

            //4. 도착지점에 도달할 때까지 반복 전진
            while (distance < FINISH_LINE)
            {
                lock (lockObject)
                {
                    waitTime = rand.Next(100, 1000);    //0.1 ~ 1초 대기시간
                }

                distance += 10; //10씩 전진(기본값)
                Thread.Sleep(waitTime); //랜덤 대기
            }

            //5. 도착 시간 기록 & 걸린 시간 계산
            DateTime endTime = DateTime.Now;    //도착 시간
            TimeSpan durationTime = endTime - startTime;    //걸린 시간

            //6. 결과 출력 (Invoke 처리)
            lock (lockObject)
            {
                int myRank = rank++;

                Invoke((MethodInvoker)(() =>
                {
                    textBox1.Text += ($"{myRank}등 - {car} 도착\r\n소요시간: {durationTime.TotalSeconds:F2}초\r\n");
                }));

                finishedCars++;

                //7. 모든 차량 도착 시 종료 메세지 출력
                if (finishedCars == cars.Length)
                {
                    Invoke((MethodInvoker)(() =>
                    {
                        textBox1.Text += ("\r\n 모든 차량 도착\r\n 경기 종료");
                    }));
                }
            }
        }
    }
}
