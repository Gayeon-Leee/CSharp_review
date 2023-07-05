namespace Day02
{
    internal class day02
    {
        static void Main(string[] args)
        {
            #region < 010 ~ 011 형식지정자 >
            /*
             Console.WriteLine()으로 출력할 때 형식지정자와 정밀도 지정자 사용 가능
             => Axx 형태로 표현, A는 형식 xx는 정밀도. 정밀도는 필수X 없으면 디폴드값 적용됨
             형식지정자는 String.Format()과 ToString()에서도 사용 가능함
             */
            decimal value = 123456.789m;
            Console.WriteLine("잔액은 {0:C2}원 입니다.", value);   // 형식지정자 C(통화), 정밀도 지정자 2(소수점 아래 두자리 출력.. 정밀도 지정자는 결과값의 자릿수에 영향) 
            Console.WriteLine("잔액은 {0,20:C2}원 입니다.", value);    // 형식지정자 C, 정밀도 지정자 2, 20으로 전체 20자리를 차지하는데 앞쪽의 사용하지 않는 부분은 빈칸으로 표시
            /* 실행 결과
             잔액은 \123,456.79원 입니다.
             잔액은          \123,456.79원 입니다.
             */
            #endregion

            #region < 013 실수 자료형 / 014 캐스팅과 자료형 변환 >
            /*
             float : 4바이트, 유효숫자 7자리 => 숫자 뒤에 접미사 f
             double : 8바이트, 유효숫자 15~16자리 => 숫자 뒤에 접미사 d.. 생략가능! 접미사 없는 숫자는 double로 인식
             decimal : 16바이트, 유효숫자 28~29자리 => 숫자 뒤에 접미사 m
             */

            // float f = 1234.5; => 접미사 없는 숫자라 double로 인식하기 때문에 float으로 선언하면 오류남
            // 하나의 자료형을 다른 자료형으로 바꾸는 것 == 형변환

            // 1. 암시적 형변환 : 작은 자료형 -> 큰 자료형으로 변환. 데이터 손실이 생기지 않기 때문에 자동으로 형변환됨
            int num = 2015051411;
            long bigNum = num; // int를 더 큰 자료형인 long으로 변환하기 때문에 암시적 형변환
            Console.WriteLine(bigNum);

            //2. 명시적 형변환 : 큰자료형 -> 작은자료형으로 변환할 때 데이터가 손실될 수 있어 강제로 형변환
            double x = 1234.5;
            int a;
            a = (int)x; // 명시적 형변환을 위해 기존 자료형을 변환하고자하는 자료형으로 캐스트하는 것. 여기서는 int로 변환되면서 소수점 아래 값은 잃어버리게됨
            Console.WriteLine(a);
            #endregion

            #region < 014 문자열과 숫자의 변환 / 015 convert 클래스>
            // 문자열 -> 숫자로 변환
            // 1. 숫자 자료형에 있는 Parse(), TryParse() 메소드 사용
            string input;
            int val;

            Console.Write("1. int로 변환할 문자열을 입력하세요 : ");
            input = Console.ReadLine();
            bool result = Int32.TryParse(input, out val);

            if (!result)
                Console.WriteLine($"{input}은 int로 변환될 수 없습니다.\n");
            else
                Console.WriteLine($"int {input}으로 변환되었습니다.\n");

            // 2. Convert 클래스 메소드 사용
            Console.Write("2. double로 변환할 문자열을 입력하세요 : ");
            input= Console.ReadLine();
            try { 
                double m = Convert.ToDouble(input); // double m = Double.Parse(input)이랑 동일하게 동작
                Console.WriteLine($"double {m}으로 변환되었습니다.\n");
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
            } // try - chatch문으로 예외처리.. 여기서는 에러메세지만

            // Convert 클래스 사용하면 ToString(), ToBoolean() 등 여러 자료형 사이의 변환도 가능
            #endregion

        }
    }
}