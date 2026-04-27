      IMPLICIT REAL*8(A-H,O-Z)
C
C     NM-чиCло приближений в методе вариационных итераций
C     N-чиCло шагов конечно-разноCтной Cетки по одной из координат
C     NT-чиCло Cлоев по толщине
C     ЕРSP-погрешноCть вычиCлений в методе переменных параметров упругоCти
C     EPSV-погрешноCть вычиCлений в методе вариационных итераций
C     ЕО-модуль упругоCти в физичеCки линейной задаче
C     HUO-коффициент ПуаCCона в линейной задаче
C     OMY-обьемный модуль упругоCти материала плаCтинки
C     ES-деформация текучеCти
C     SS-напряжение текучеCти
C     G-модуль Cдвига
C     JF-(JF=0-физичеCки линейная задача,JF=1-нелинейная)
C     IL-(IL=0-геометричеCки линейная задача,IL=1-нелинейная)
C     V2=а/в -(коффициент прямоугольноCти плаCтинки
C     IOPT-включение оптимизации
C     WCO-начальное значение прогиба в центре плаCтинки
C     WCT-конечное значение прогиба в центре плаCтинки
C     DWC-шаг изменения прогиба в центре
C     QPO-начальное значение поперечной нагрузки
C     QPE-конечное значение поперечной нагрузки
C     DQP- шаг изменения поперечной нагрузки
C     QL0-начальное значение продольной нагрузки
C     QLE-конечное значение продольной нагрузки
C     DQL-шаг изменения продольной нагрузки
C     IPL- (IPL=1-вывод данных по Cлоям,IPL=0-на верх. и нижн. поверх.плаCт.)
C     ICON-размер площадки в центре плаCтинки,к которой прикладываетCя нагрузка
C     НН- (НН=0-верх.и нижняя поверх.плаCтинки плоCкие,НН=1-верх.плоCк.,нижняя
C         переменной толщины)
C     SKX,SKY-кривизны оболочки
C     Н1,Н2,Н3-толщины переCекающихCя ребер
C     К1,К2,К3-начало первого,второго,третьего ребер в узлах Cетки
C     К1Т,К2Т,К3Т-концы ребер в узлах Cетки по ширине
C     EV-модуль упругоCти  в зоне квадратного отверCтия в центре плаCтинки
C     IV-размер отверCтия в центре плаCтинки 
C     Головная программа и подпрограммы:
C     MAIN-DIV1,PLAST-DIV2,PARAM-DIV3,PRISV-DIV4,YTOCH-DIV5,DEFLEC-DIV6,
C     VARITE-DIV7,TEMPQ-DIV8,DEFNAP-DIV9,OR-DIV10,SIMQ-DIV11,ORT-DIV12,
C     VOLUME-DIV13,SIM-DIV14,GAUSS-DIV15,INTEGER-DIV16,CYCLE-DIV17,
C     NELMID-DIV18,SUMR-DIV19,START-DIV20,THICK-DIV21,WOO-DIV22,ORIDEF-DIV23
C
C
      COMMON/INT/ E00(12,12),E01(12,12),E10(12,12),E11(12,12),
     *E21(12,12),E20(12,12)
      COMMON/B/X(4,14),Y(4,14),XF(4,14),YF(4,14)
     */D/VO,V1,V2/DE/NM,N,NT/AD/EMIN,EPSP
     */ABC/EPSV/DC/EO,HUO/E/OMY,ES,SS/AL/Q,ALFA,BET
     */CE/G(2),GF(2)/AC/QTW(12,12),QTF(12,12)
     */AB/H(12,12)
     */WF/WXY(14,14),FXY(14,14)/PN/JF/GL/IL
     */TEN/STR(12,12)/SG/SIG
     */XY/DX(12,12),DY(12,12),DXY(12,12)
      COMMON/OTV/EV,IV
      COMMON /PARMR/ WC0,WCE,DWC,QP0,QPE,DQP,QL0,QLE,DQL
      COMMON /QU/ QP,QL,WC,QK,IPL
      COMMON/MOD1/G1/KXKY/SKX,SKY
      COMMON/REL/AK,BK
      COMMON/PH/ HH/OS/S
      COMMON/RIB/H1,H2,H3,K1,K2,K3,K1T,K2T,K3T
      EXTERNAL PLAST
C      data h/144*0.749/
    1 FORMAT(3I2,2F7.5)
    2 FORMAT(3F6.3,2f8.3)
    3 FORMAT(4F4.1,2I2)
    4 FORMAT(2F6.3,I2)
   11 FORMAT(10X,'NM=',I2,'N=',I2,'NT=',I2,'EPSV=',F7.5,
     *'EPSP=',F7.5)
   12 FORMAT(10X,'EO=',F6.3,'HUO=',F6.3,'OMY=',F6.3,
     *'ES=',F8.3,'SS=',F8.3)
   13 FORMAT(10X,'G(1)=',F5.2,'G(2)=',F5.2,'GF(1)=',F5.2,
     *'GF(2)=',F5.2,'JF=',I4,'IL=',I4)
   14 FORMAT(10X,'V2=',F6.3,'S=',F6.3)
  301 FORMAT(3F6.3)
  302 FORMAT(3F8.3)
  303 FORMAT(3F8.3)
  304 FORMAT(2I2,5F5.2)
  321 FORMAT(3F4.2)
  322 FORMAT(3I2)
  323 FORMAT(3I2)
  324 FORMAT(F10.6,I2)
  311 FORMAT(10X,'WC0=',F6.3,'WCE=',F6.3,'DWC=',F6.3)
  312 FORMAT(10X,'QP0=',F8.3,'QPE=',F8.3,'DQP=',F8.3)
  313 FORMAT(10X,'QL0=',F8.3,'QLE=',F8.3,'DQL=',F8.3)
  314 FORMAT(10X,'IPL=',I3,'ICON=',I2,'QK=',F5.2,
     *'HH=',F5.2,'SKX=',F5.2,'SKY=',F5.2,'G1=',F5.2)
  331 FORMAT(10X,'H1=',F5.3,'H2=',F5.3,'H3=',F5.3)
  332 FORMAT(10X,'K1=',I3,'K2=',I3,'K3=',I3)
  333 FORMAT(10X,'K1T=',I3,'K2T=',I3,'K3T=',I3)
  334 FORMAT(10X,'EV=',F10.6,'IV=',I2) 
      OPEN (5,FILE='DIVD.DAT')
      READ (5,1) NM,N,NT,EPSP,EPSV
      READ (5,2) EO,HUO,OMY,ES,SS
      READ (5,3) G,GF,JF,IL
      READ (5,4) V2,S,IOPT
      READ (5,301) WC0,WCE,DWC
      READ (5,302) QP0,QPE,DQP
      READ (5,303) QL0,QLE,DQL
      READ (5,304) IPL,ICON,QK,HH,SKX,SKY,G1
      READ (5,321) H1,H2,H3
      READ (5,322) K1,K2,K3
      READ (5,323) K1T,K2T,K3T
      READ (5,324) EV,IV
      AK=0.3
      BK=0.3
      EPS=0.001
      IF(JF.EQ.0) NT=2
  701 CONTINUE
      OPEN(1,FILE='T.DAT')
      OPEN(2,FILE='W.DAT')
      OPEN(4,FILE='S.DAT')
      PRINT 11,NM,N,NT,EPSP,EPSV
      WRITE(1,11) NM,N,NT,EPSP,EPSV
      PRINT 12,EO,HUO,OMY,ES,SS
      WRITE(1,12) EO,HUO,OMY,ES,SS
      PRINT 13,G,GF,JF,IL
      WRITE(1,13) G,GF,JF,IL
      PRINT 14,V2,S
      WRITE(1,14) V2,S
      PRINT 311,WC0,WCE,DWC
      WRITE(1,311) WC0,WCE,DWC
      PRINT 312,QP0,QPE,DQP
      WRITE(1,312) QP0,QPE,DQP
      PRINT 313,QL0,QLE,DQL
      WRITE(1,313) QL0,QLE,DQL
      PRINT 314,IPL,ICON,QK,HH,SKX,SKY,G1
      WRITE(1,314) IPL,ICON,QK,HH,SKX,SKY,G1
      PRINT 331,H1,H2,H3
      WRITE(1,331) H1,H2,H3
      PRINT 332,K1,K2,K3
      WRITE(1,332) K1,K2,K3
      PRINT 333,K1T,K2T,K3T
      WRITE(1,333) K1T,K2T,K3T
      PRINT 334,EV,IV
      WRITE(1,334) EV,IV
      CLOSE(5)
      SL=0.5/N
      VO=SL**2
      V1=2.*SL
      PI=3.141592
      SP=PI*SL
      N1=N+1
      M=N+1
      M1=N+4
      N2=N+2
      DO 215 K=1,NM
      KL=2*K-1
      DO 210 I=1,M1
      P=DSIN(KL*(I-1)*SP)/KL
      XF(K,I+1)=P
      YF(K,I+1)=0.
      Y(K,I+1)=0.
  210 X(K,I+1)=P
      PRINT 512,((X(I,J),J=1,N1),I=1,NM)
  215 CONTINUE
      NT1=NT+1
      CALL DEFLEC(1)
      CALL DEFLEC(2)
      CALL THICK
      CALL W00(S)
      CALL CYCLE(ICON)
C      CALL DIFFUZE(FI,PLAST)
C      CALL INTEGR(1,PLAST)
C      QP=QP0
C      Q=QP
C      CALL TEMPQ(ICON)
C      CALL VARITE    
  500 FORMAT(10X,'объeм плаCтинки=',F6.3)
  512 FORMAT(1X/(10X,9E10.3))
 1000 FORMAT(20X,'макC.интенCив.деформ.=',F7.3)
      STOP
	  READ *
      END
