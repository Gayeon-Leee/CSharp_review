# CSharp_review
C# 개인 공부 및 프로젝트 리포지토리

## C# 200제
	- 1일차 : 001 ~ 009
	- 2일차 : 010 ~ 018 : 연산자 및 zero division error는 코드 작성 X


## Smart Building Project - 관리자용 WPF 앱 개발
### 1일차(7.6)
- WPF 템플릿 제작
- 차량 관리 화면(진행 중)

### 2일차(7.7)
- 차량 관리 화면 완료
	- 차량 등록, 수정, 삭제 => MySQL DB 바인딩 완료
- 로그인 화면 완료
	- MySQL 관리자 DB 바인딩 완료
	- 앱 구현 마지막 단계에서 로드 이벤트 추가 예정

### 3일차(7.11)
- 센서 제어 대시보드 정리(진행 중)
- 날씨 화면 제작(진행 중)

### 4일차(7.12)
- 날씨 화면 제작(진행 중)
	- 기상청 API 연동 완료
	- 온도, 습도, 풍속 textblock에 바인딩 완료

### 5일차 (7.13)
- 날씨 화면 제작(진행 중)
	- 기상청 API 연동 완료
	- 온도, 습도, 풍속 textblock에 바인딩 완료
	- 값에 따른 날씨 이미지 변경 완료
	- 추가 진행사항 : 대시보드 정리

### 6일차 (7.14)
- 현재 단계에서 필요한 화면은 구현 완료
	- 라즈베리파이 <-> 아두이노 시리얼 통신 완료 
	- 라즈베리파이 <-> 윈도우(WPF 앱) MQTT 통신 확인 필요
	- 통신 완료시 나머지 화면 제작

<img src="https://raw.githubusercontent.com/Gayeon-Leee/CSharp_review/main/Images/smartbuilding1.jpg" width="700"/>

### 7일차 (7.15)
- 아두이노 <- 시리얼 통신 -> 라즈베리파이 <- MQTT -> WPF 앱 확인 완료
	- WPF 앱에서 토글버튼으로 아두이노에 연결된 LED 제어 가능
<img src="https://raw.githubusercontent.com/Gayeon-Leee/CSharp_review/main/Images/smartbuilding2.gif" width="700" height="400"/>

### 8일차 (7.16)
- Timer 사용 -> 실행 중 현재 시간 실시간으로 바뀜
- 추가 작업은 센서 및 기능 구현 내용 정리 후 필요

### 9일차 (7.17)
- 기상청 홈페이지 크롤링 테스트
	- 기존 API 오류 잦아 홈페이지 크롤링 테스트
	- string으로 받아오는 값 double 형식으로 변환해야하는데 오류남... 확인 필요

### 10일차 (7.18)
- 기상청 홈페이지 크롤링 완료
	- 기상청 API 대신 웹크롤링해서 받아온 값으로 날씨영역 바인딩

- 대시보드 정리
<img src="https://raw.githubusercontent.com/Gayeon-Leee/CSharp_review/main/Images/smartbuilding3.png" width="700"/>
 	
### 11일차 (7.24)
- 사용자용 모니터링 앱 정리 진행 중
- MQTT 통신 테스트
  - 토글 버튼으로 LED 단순 제어 가능

### 12일차 (7.25)
- 사용자용 모니터링 앱 정리 진행 중
  - 온습도 센서 화면 구현 완료

### 13일차 (7.26)
- 사용자용 모니터링 앱 정리
- MQTT 통신
  - 온습도값 바인딩
  - LED 제어 가능

  
 < ToDoList >
- [ ] MQTT 통신 확인
  - 펜모터, 화재감지센서 등 전체 센서 제어 필요 
