using System.Globalization;

namespace Day01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region < 003 콘솔에서 읽고 쓰기 / 004 변수 선언 및 자료형>
            Console.Write("Hello ");    // 콘솔에 문자열 출력
            Console.WriteLine("World!");    // 콘솔에 문자열 출력하고 줄바꿈
            
            Console.Write("이름을 입력하세요: ");
            string name = Console.ReadLine();   // string 타입 변수 name 선언, 콘솔에서 입력받은 값 변수에 할당
            Console.Write("나이를 입력하세요: ");
            int age = int.Parse(Console.ReadLine()); // Console.ReadLine()은 콘솔에 입력되는 값 string으로 반환 => int로 변환해서 변수에 할당
            Console.Write("키를 입력하세요: ");
            float height = float.Parse(Console.ReadLine()); // 문자열을 float으로 변환하여 변수에 할당

            Console.Write("안녕하세요, ");
            Console.Write(name);
            Console.WriteLine("님!");

            Console.WriteLine($"나이는 {age}세, 키는 {height}cm 이군요!"); // Console.Write와 Console.WriteLine 사용 줄이기 위해 포맷팅 
            #endregion

            #region < 005 문자와 문자열 >
            string a = "hello";
            string b = "h";

            b = b + "ello";
            Console.WriteLine(a == b); // a와 b를 비교 => 같기 때문에 true 출력
            Console.WriteLine("b = " + b);

            int c = 10;
            string d = b + '!' + " " + c; 
            // string 타입에 다른 자료형 값을 연산자 +로 연결하면 다른 자료형이 string으로 바뀌어 연결됨
            Console.WriteLine($"d = {d}");
            // int x가 string으로 바뀌어 연결,저장되어 hello! 10 출력
            #endregion

            #region < 006 대입연산자와 대입문 >
            int i = 5;
            double x = 3.141592;
            Console.WriteLine($"i = {i}, x = {x}");

            x = i;  // double은 8바이트, int는 4바이트로 값 손실 없이 int x를 double i에 암시적으로 형변환하여 할당
            i = (int)x; // 더 큰 자료형을 작은 자료형으로 변환하기때문에 명시적으로 (int) 사용하여 형변환 ==> 캐스트(cast)
            Console.WriteLine($"i = {i}, x = {x}");
            #endregion

            #region< 007 ~ 009 / Console.WriteLine 메소드>
            // 007 Console.Write() / Console.WriteLine()은 하나의 변수나 값을 출력할 때는 어떤 자료형이라도 출력 가능
            
            // 008 여러 개의 값을 출력
            Console.WriteLine("10 이하의 소수 : {0}, {1}, {2}, {3}", 2, 3, 5, 7); // {0}, {1}, {2}, {3} 으로 파라미터 지정

            string primes;
            primes = String.Format("10 이하의 소수 : {0}, {1}, {2}, {3}", 2, 3, 5, 7); // String.Farmat() 사용시 콘솔 출력하는것과 똑같이 문자열에 저장 가능
            Console.WriteLine(primes);

            // 009 자료형이 다른 두 개 이상의 변수 출력
            int v1 = 100;
            double v2 = 1.234;
            // Console.WriteLine(v1, v2); 자료형이 다르면 이런식으로 그냥 출력할 수 없음!
            // 1. 변수 값을 각각 문자열로 바꿔 연결해서 하나의 문자열로 출력
            Console.WriteLine(v1.ToString() + ", " + v2.ToString()); // ToString()으로 문자열 변환 후 + 연산자로 연결
            Console.WriteLine("v1 = " + v1 + ", v2 = " + v2); // "v1 = " ", v2 = " 문자열에 + 연산자로 연결해서 하나의 문자열로 만들어 출력
            // 2. 형식(format) 정보를 사용해서 여러 개의 변수나 값을 출력
            Console.WriteLine("v1 = {0}, v2 = {1}", v1, v2);
            // 3. 문자열 보간(string interpolation)
            Console.WriteLine($"v1 = {v1}, v2 = {v2}");
            #endregion
        }
    }
}