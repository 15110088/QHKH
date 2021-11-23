create or replace PROCEDURE ST_BieuMau01CT(IDKYQH in number,preloop out SYS_REFCURSOR)
AS BEGIN
open preloop for 
select KIHIEU,STT,DM_KVHC.MA_KVHC as MAHUYEN,case when sum(HIENTRANG.DIENTICH) is null then 0 else ROUND(sum(HIENTRANG.DIENTICH)/10000,2) end as DIENTICH  
from DM_KVHC 
 join DM_MUCDICHSUDUNG on  DM_MUCDICHSUDUNG.ID!=MA_KVHC
 left join HIENTRANG on DM_MUCDICHSUDUNG.ID=HIENTRANG.ID_MDSD and hientrang.mahuyen=DM_KVHC.MA_KVHC and HIENTRANG.captinh=1 
 where  ID_CAP_KVHC=2  and dm_mucdichsudung.captinh=1  
 and (ID_KYQH=IDKYQH or ID_KYQH is null)
group by KIHIEU,DM_KVHC.MA_KVHC,STT
order by  dm_mucdichsudung.STT;
Exception
When No_Data_Found Then
     -- Tr??ng h?p câu SELECT không tr? v? b?n ghi nào
     Dbms_Output.Put_Line('No data with emp_id= ');
END;


create or replace PROCEDURE ST_BieuMau02CT(IDKYQH in number,preloop out SYS_REFCURSOR)
AS BEGIN
open preloop for 
select KIHIEU,STT,' ' as MAHUYEN,case when sum(DIENTICH) is null then 0 else ROUND(sum(DIENTICH)/10000,2) end as DIENTICH  
from DM_MUCDICHSUDUNG  
left join KEHOACH on DM_MUCDICHSUDUNG.ID=KEHOACH.ID_MDSD
where dm_mucdichsudung.tt01=1 and dm_mucdichsudung.captinh=1
and (ID_KYQH=IDKYQH or ID_KYQH is null)
group by KIHIEU,STT
order by dm_mucdichsudung.STT;
Exception
When No_Data_Found Then
     -- Tr??ng h?p câu SELECT không tr? v? b?n ghi nào
     Dbms_Output.Put_Line('No data with emp_id= ');
END;

create or replace PROCEDURE ST_BieuMau03CT(IDKYQH in number,preloop out SYS_REFCURSOR)
AS BEGIN
open preloop for 
select KIHIEU,STT,' ' as MAHUYEN,case when sum(PHANBO_XACDINH.DT_PHANBO) is null then 0 else ROUND(sum(PHANBO_XACDINH.DT_PHANBO)/10000,2) end as DT_PHANBO ,case when sum(PHANBO_XACDINH.DT_XACDINH) is null then 0 else ROUND(sum(PHANBO_XACDINH.DT_XACDINH)/10000,2) end as DT_XACDINH  
from DM_MUCDICHSUDUNG  
left join PHANBO_XACDINH on DM_MUCDICHSUDUNG.ID=PHANBO_XACDINH.ID_MDSD and PHANBO_XACDINH.captinh=1
where dm_mucdichsudung.tt01=1 and dm_mucdichsudung.captinh=1
and (PHANBO_XACDINH.ID_KYQH =3 or PHANBO_XACDINH.id_kyqh is null)
group by KIHIEU,STT
order by dm_mucdichsudung.STT;
Exception
When No_Data_Found Then
     -- Tr??ng h?p câu SELECT không tr? v? b?n ghi nào
     Dbms_Output.Put_Line('No data with emp_id= ');
END;

create or replace PROCEDURE ST_BieuMau03CT_KEHOACH(IDKYQH in number,preloop out SYS_REFCURSOR)
AS BEGIN
open preloop for 
select KIHIEU,STT,DM_KVHC.MA_KVHC as MAHUYEN,case when sum(kehoach.DIENTICH) is null then 0 else ROUND(sum(kehoach.DIENTICH)/10000,2) end as DIENTICH  
from DM_KVHC 
 join DM_MUCDICHSUDUNG on  DM_MUCDICHSUDUNG.ID!=MA_KVHC
 left join kehoach on DM_MUCDICHSUDUNG.ID=kehoach.ID_MDSD and kehoach.mahuyen=DM_KVHC.MA_KVHC and kehoach.captinh=1 
 where  ID_CAP_KVHC=2  and dm_mucdichsudung.captinh=1  
 and (ID_KYQH=IDKYQH or ID_KYQH is null)
group by KIHIEU,DM_KVHC.MA_KVHC,STT
order by  dm_mucdichsudung.STT;
Exception
When No_Data_Found Then
     -- Tr??ng h?p câu SELECT không tr? v? b?n ghi nào
     Dbms_Output.Put_Line('No data with emp_id= ');
END;



create or replace PROCEDURE ST_BieuMau04CT(IDKYQH in number,preloop out SYS_REFCURSOR)
AS BEGIN
open preloop for 

select KIHIEU,STT,CAST(GETNAM.NAM AS NVARCHAR2(4))   as MAHUYEN,case when sum(kehoach.DIENTICH) is null then 0 else ROUND(sum(kehoach.DIENTICH)/10000,2) end as DIENTICH  
 from 

 (
        DM_MUCDICHSUDUNG
        right join (select kehoach.NAM from   kehoach 
        join kyquyhoachkehoach on kehoach.id_kyqh=kyquyhoachkehoach.ID
        where kehoach.NAM >= (select TUNAM from  kyquyhoachkehoach
        where ID=IDKYQH) and 
        kehoach.NAM <= (select TOINAM from  kyquyhoachkehoach
        where ID=IDKYQH ))  GETNAM  on   DM_MUCDICHSUDUNG.ID!=-4
        left join kehoach on DM_MUCDICHSUDUNG.ID=kehoach.ID_MDSD  and kehoach.captinh=1  and kehoach.nam=GETNAM.NAM
 )
 where  dm_mucdichsudung.captinh=1  
 and
 (kehoach.NAM >= (select TUNAM from  kyquyhoachkehoach
where ID=IDKYQH) and 
kehoach.NAM <= (select TOINAM from  kyquyhoachkehoach
where ID=IDKYQH

) or kehoach.id_kyqh is null)

group by KIHIEU,STT,GETNAM.NAM
order by  dm_mucdichsudung.STT;


Exception
When No_Data_Found Then
     -- Tr??ng h?p câu SELECT không tr? v? b?n ghi nào
     Dbms_Output.Put_Line('No data with emp_id= ');
END;

create or replace PROCEDURE ST_BieuMau04CT_HIENTRANG(IDKYQH in number,preloop out SYS_REFCURSOR)
AS BEGIN
open preloop for 
select KIHIEU,STT,CAST(EXTRACT(YEAR FROM sysdate) AS NVARCHAR2(4))  as MAHUYEN,case when sum(hientrang.DIENTICH) is null then 0 else ROUND(sum(hientrang.DIENTICH)/10000,2) end as DIENTICH  
 from 


        DM_MUCDICHSUDUNG

        left join hientrang on DM_MUCDICHSUDUNG.ID=hientrang.ID_MDSD  and hientrang.captinh=1  and hientrang.nam=EXTRACT(YEAR FROM sysdate) 

 where  dm_mucdichsudung.captinh=1  
 and
 (hientrang.NAM >= (select TUNAM from  kyquyhoachkehoach
where ID=IDKYQH) and 
hientrang.NAM <= (select TOINAM from  kyquyhoachkehoach
where ID=IDKYQH

) or hientrang.id_kyqh is null)

group by KIHIEU,STT,''
order by  dm_mucdichsudung.STT;
Exception
When No_Data_Found Then
     -- Tr??ng h?p câu SELECT không tr? v? b?n ghi nào
     Dbms_Output.Put_Line('No data with emp_id= ');
END;


create or replace PROCEDURE ST_BieuMau07CT(IDKYQH in number,preloop out SYS_REFCURSOR)
AS BEGIN
open preloop for 
select KIHIEU,STT,DM_KVHC.MA_KVHC as MAHUYEN,case when sum(kehoach_csd.DIENTICH) is null then 0 else ROUND(sum(kehoach_csd.DIENTICH)/10000,2) end as DIENTICH  
from DM_KVHC 
 join DM_MUCDICHSUDUNG on  DM_MUCDICHSUDUNG.ID!=MA_KVHC
 left join kehoach_csd on DM_MUCDICHSUDUNG.ID=kehoach_csd.ID_MDSD and kehoach_csd.mahuyen=DM_KVHC.MA_KVHC and kehoach_csd.captinh=1 
 where  ID_CAP_KVHC=2  and dm_mucdichsudung.captinh=1  
 and (ID_KYQH=IDKYQH or ID_KYQH is null)
group by KIHIEU,DM_KVHC.MA_KVHC,STT
order by  dm_mucdichsudung.STT;
Exception
When No_Data_Found Then
     -- Tr??ng h?p câu SELECT không tr? v? b?n ghi nào
     Dbms_Output.Put_Line('No data with emp_id= ');
END;


create or replace PROCEDURE ST_BieuMau05CT(IDKYQH in number,preloop out SYS_REFCURSOR)
AS BEGIN
open preloop for 
select KYHIEU,DM_CHUYENMUCDICH.ID as STT,DM_KVHC.MA_KVHC as MAHUYEN,case when sum(KEHOACH_CMD.DIENTICH) is null then 0 else ROUND(sum(KEHOACH_CMD.DIENTICH)/10000,2) end as DIENTICH  
from DM_KVHC 
 join DM_CHUYENMUCDICH on  DM_CHUYENMUCDICH.ID!=MA_KVHC
 left join KEHOACH_CMD on DM_CHUYENMUCDICH.ID=KEHOACH_CMD.ID_CMD and KEHOACH_CMD.mahuyen=DM_KVHC.MA_KVHC and KEHOACH_CMD.captinh=1 
 where  ID_CAP_KVHC=2  and DM_CHUYENMUCDICH.captinh=1  
 and (ID_KYQH=IDKYQH or ID_KYQH is null)
group by KYHIEU,DM_KVHC.MA_KVHC,DM_CHUYENMUCDICH.ID
order by  DM_CHUYENMUCDICH.ID;
Exception
When No_Data_Found Then
     -- Tr??ng h?p câu SELECT không tr? v? b?n ghi nào
     Dbms_Output.Put_Line('No data with emp_id= ');
END;


create or replace PROCEDURE ST_BieuMau10CT(IDKYQH in number,preloop out SYS_REFCURSOR)
AS BEGIN
open preloop for 
select dm_loaikhuchucnang.kyhieu as MAHUYEN,dm_mucdichsudung.KIHIEU,STT,case when sum(khuchucnang_mdsd.DIENTICH) is null then 0 else ROUND(sum(khuchucnang_mdsd.DIENTICH)/10000,2) end as DIENTICH

from DM_LOAIKHUCHUCNANG 
 join DM_MUCDICHSUDUNG on  DM_MUCDICHSUDUNG.ID!=-4
 left join KHUCHUCNANG on     KHUCHUCNANG.MALOAIKHUCN=DM_LOAIKHUCHUCNANG.ID
 left join khuchucnang_mdsd on DM_MUCDICHSUDUNG.ID=khuchucnang_mdsd.ID_MDSD  and khuchucnang_mdsd.id_khucn=  KHUCHUCNANG.ID

 where  dm_mucdichsudung.captinh=1  and DM_LOAIKHUCHUCNANG.captinh=1 and (ID_KYQH=IDKYQH or ID_KYQH is null)
group by KIHIEU,DM_LOAIKHUCHUCNANG.kyhieu,STT,khuchucnang.MAKHUCN

 order by  dm_mucdichsudung.STT;
Exception
When No_Data_Found Then
     -- Tr??ng h?p câu SELECT không tr? v? b?n ghi nào
     Dbms_Output.Put_Line('No data with emp_id= ');
END;




--Bieu mau cap tinh lam bi sai khong co lay duoc ky truoc
create or replace PROCEDURE ST_BieuMau02CT(IDKYQH in number,preloop out SYS_REFCURSOR)
AS BEGIN
open preloop for 
select KIHIEU,STT,' ' as MAHUYEN,case when sum(DIENTICH) is null then 0 else ROUND(sum(DIENTICH)/10000,2) end as DIENTICH  
from DM_MUCDICHSUDUNG  
left join KEHOACH on DM_MUCDICHSUDUNG.ID=KEHOACH.ID_MDSD

where dm_mucdichsudung.tt01=1 and dm_mucdichsudung.captinh=1
and (ID_KYQH=IDKYQH or ID_KYQH is null)
group by KIHIEU,STT
order by dm_mucdichsudung.STT;
Exception
When No_Data_Found Then
     -- Tr??ng h?p câu SELECT không tr? v? b?n ghi nào
     Dbms_Output.Put_Line('No data with emp_id= ');
END;


-- CH
create or replace PROCEDURE ST_BieuMau01CH(IDKYQH in number,IDHUYEN in varchar,preloop out SYS_REFCURSOR)
AS BEGIN
open preloop for 
select KIHIEU,STT,DM_KVHC.MA_KVHC as MAHUYEN,case when sum(HIENTRANG.DIENTICH) is null then 0 else ROUND(sum(HIENTRANG.DIENTICH)/10000,2) end as DIENTICH  
from DM_KVHC 
 join DM_MUCDICHSUDUNG on  DM_MUCDICHSUDUNG.ID!=-1
 left join HIENTRANG on DM_MUCDICHSUDUNG.ID=HIENTRANG.ID_MDSD and hientrang.mahuyen=DM_KVHC.MA_KVHC and HIENTRANG.captinh=0
 where  ID_CAP_KVHC=1  and STT is not null and DM_KVHC.MA_KVHC_CHA=IDHUYEN
 and (ID_KYQH=IDKYQH or ID_KYQH is null)
group by KIHIEU,DM_KVHC.MA_KVHC,STT
order by  dm_mucdichsudung.STT,DM_KVHC.MA_KVHC;
Exception
When No_Data_Found Then
     -- Tr??ng h?p câu SELECT không tr? v? b?n ghi nào
     Dbms_Output.Put_Line('No data with emp_id= ');
END;

create or replace PROCEDURE ST_BieuMau02CH(IDKYQH in number,IDHUYEN in varchar,preloop out SYS_REFCURSOR)
AS BEGIN
open preloop for 

select  KIHIEU,STT,DM_KVHC.MA_KVHC_CHA,case when sum(KEHOACH.DIENTICH) is null then 0 else ROUND(sum(KEHOACH.DIENTICH)/10000,2) end as DIENTICH 

from DM_KVHC 
join DM_MUCDICHSUDUNG on  DM_MUCDICHSUDUNG.ID!=-1
left join KEHOACH on DM_MUCDICHSUDUNG.ID=KEHOACH.ID_MDSD and KEHOACH.captinh=0 and  DM_KVHC.MA_KVHC= KEHOACH.maxa
where dm_mucdichsudung.tt01=1
and  ID_CAP_KVHC=1
and STT is not null and DM_KVHC.MA_KVHC_CHA=IDHUYEN 
and (kehoach.ID_KYQH in (select ID_CHA from  kyquyhoachkehoach
where ID=IDKYQH) or kehoach.id_kyqh is null)
group by KIHIEU,STT,DM_KVHC.MA_KVHC_CHA
order by  dm_mucdichsudung.STT;


Exception
When No_Data_Found Then
     -- Tr??ng h?p câu SELECT không tr? v? b?n ghi nào
     Dbms_Output.Put_Line('No data with emp_id= ');
END;

create or replace PROCEDURE ST_BieuMau05CH(IDKYQH in number,IDHUYEN in varchar,preloop out SYS_REFCURSOR)
AS BEGIN
open preloop for 
select KIHIEU,STT,DM_KVHC.MA_KVHC as MAHUYEN,case when sum(kehoach_csd.DIENTICH) is null then 0 else ROUND(sum(kehoach_csd.DIENTICH)/10000,2) end as DIENTICH  
from DM_KVHC 
 join DM_MUCDICHSUDUNG on  DM_MUCDICHSUDUNG.ID!=-1
 left join kehoach_csd on DM_MUCDICHSUDUNG.ID=kehoach_csd.ID_MDSD and kehoach_csd.maxa=DM_KVHC.MA_KVHC and kehoach_csd.captinh=0
 where  ID_CAP_KVHC=1  and STT is not null and DM_KVHC.MA_KVHC_CHA=IDHUYEN
 and (ID_KYQH=IDKYQH or ID_KYQH is null)
group by KIHIEU,DM_KVHC.MA_KVHC,STT
order by  dm_mucdichsudung.STT,DM_KVHC.MA_KVHC;
Exception
When No_Data_Found Then
     -- Tr??ng h?p câu SELECT không tr? v? b?n ghi nào
     Dbms_Output.Put_Line('No data with emp_id= ');
END;


create or replace PROCEDURE ST_BieuMau06CH(IDKYQH in number,IDHUYEN in varchar,preloop out SYS_REFCURSOR)
AS BEGIN
open preloop for 

select  KIHIEU,STT,DM_KVHC.MA_KVHC_CHA,case when sum(KEHOACH.DIENTICH) is null then 0 else ROUND(sum(KEHOACH.DIENTICH)/10000,2) end as DIENTICH 

from DM_KVHC 
join DM_MUCDICHSUDUNG on  DM_MUCDICHSUDUNG.ID!=-1
left join KEHOACH on DM_MUCDICHSUDUNG.ID=KEHOACH.ID_MDSD and KEHOACH.captinh=0 and  DM_KVHC.MA_KVHC= KEHOACH.maxa
where dm_mucdichsudung.tt01=1
and  ID_CAP_KVHC=1
and STT is not null and DM_KVHC.MA_KVHC_CHA=IDHUYEN 
and (ID_KYQH=IDKYQH or ID_KYQH is null)
group by KIHIEU,STT,DM_KVHC.MA_KVHC_CHA
order by  dm_mucdichsudung.STT;


Exception
When No_Data_Found Then
     -- Tr??ng h?p câu SELECT không tr? v? b?n ghi nào
     Dbms_Output.Put_Line('No data with emp_id= ');
END;

create or replace PROCEDURE ST_BieuMau07CH(IDKYQH in number,IDHUYEN in varchar,preloop out SYS_REFCURSOR)
AS BEGIN
open preloop for 
select case when KYHIEU is null then ' ' else KYHIEU end as KIHIEU    ,DM_CHUYENMUCDICH.ID as STT,DM_KVHC.MA_KVHC as MAHUYEN
,case when sum(KEHOACH_CMD.DIENTICH) is null then 0 else ROUND(sum(KEHOACH_CMD.DIENTICH)/10000,2) end as DIENTICH  
from DM_KVHC 
 join DM_CHUYENMUCDICH on  DM_CHUYENMUCDICH.ID!=-4
 left join KEHOACH_CMD on DM_CHUYENMUCDICH.ID=KEHOACH_CMD.ID_CMD and KEHOACH_CMD.maxa=DM_KVHC.MA_KVHC and KEHOACH_CMD.captinh=0 
 where  ID_CAP_KVHC=1 and   dm_kvhc.ma_kvhc_cha=IDHUYEN 
 and (ID_KYQH=IDKYQH or ID_KYQH is null)
group by KYHIEU,DM_KVHC.MA_KVHC,DM_CHUYENMUCDICH.ID
order by  DM_CHUYENMUCDICH.ID;
Exception
When No_Data_Found Then
     -- Tr??ng h?p câu SELECT không tr? v? b?n ghi nào
     Dbms_Output.Put_Line('No data with emp_id= ');
END;

create or replace PROCEDURE ST_BieuMau08CH(IDKYQH in number,IDHUYEN in varchar,preloop out SYS_REFCURSOR)
AS BEGIN
open preloop for 
select KIHIEU,STT,DM_KVHC.MA_KVHC as MAHUYEN,case when sum(KEHOACH_THUHOI.DIENTICH) is null then 0 else ROUND(sum(KEHOACH_THUHOI.DIENTICH)/10000,2) end as DIENTICH  
from DM_KVHC 
 join DM_MUCDICHSUDUNG on  DM_MUCDICHSUDUNG.ID!=-1
 left join KEHOACH_THUHOI on DM_MUCDICHSUDUNG.ID=KEHOACH_THUHOI.ID_MDSD and KEHOACH_THUHOI.maxa=DM_KVHC.MA_KVHC 
 where  ID_CAP_KVHC=1  and STT is not null and DM_KVHC.MA_KVHC_CHA=IDHUYEN
 and (ID_KYQH=IDKYQH or ID_KYQH is null)
group by KIHIEU,DM_KVHC.MA_KVHC,STT
order by  dm_mucdichsudung.STT,DM_KVHC.MA_KVHC;
Exception
When No_Data_Found Then
     -- Tr??ng h?p câu SELECT không tr? v? b?n ghi nào
     Dbms_Output.Put_Line('No data with emp_id= ');
END;

create or replace PROCEDURE ST_BieuMau09CH(IDKYQH in number,IDHUYEN in varchar,preloop out SYS_REFCURSOR)
AS BEGIN
open preloop for 
select KIHIEU,STT,DM_KVHC.MA_KVHC as MAHUYEN,case when sum(kehoach_csd.DIENTICH) is null then 0 else ROUND(sum(kehoach_csd.DIENTICH)/10000,2) end as DIENTICH  
from DM_KVHC 
 join DM_MUCDICHSUDUNG on  DM_MUCDICHSUDUNG.ID!=-1
 left join kehoach_csd on DM_MUCDICHSUDUNG.ID=kehoach_csd.ID_MDSD and kehoach_csd.maxa=DM_KVHC.MA_KVHC and kehoach_csd.captinh=0
 where  ID_CAP_KVHC=1  and STT is not null and DM_KVHC.MA_KVHC_CHA=IDHUYEN
 and (ID_KYQH=IDKYQH or ID_KYQH is null)
group by KIHIEU,DM_KVHC.MA_KVHC,STT
order by  dm_mucdichsudung.STT,DM_KVHC.MA_KVHC;
Exception
When No_Data_Found Then
     -- Tr??ng h?p câu SELECT không tr? v? b?n ghi nào
     Dbms_Output.Put_Line('No data with emp_id= ');
END;

create or replace PROCEDURE ST_BieuMau11CH(IDKYQH in number,IDHUYEN in varchar,preloop out SYS_REFCURSOR)
AS BEGIN
open preloop for 

select dm_loaikhuchucnang.kyhieu,dm_mucdichsudung.KIHIEU,dm_mucdichsudung.STT,case when sum(khuchucnang_mdsd.DIENTICH) is null then 0 else ROUND(sum(khuchucnang_mdsd.DIENTICH)/10000,2) end as DIENTICH

from DM_LOAIKHUCHUCNANG 
 join DM_MUCDICHSUDUNG on  DM_MUCDICHSUDUNG.ID!=-4
 left join KHUCHUCNANG on     KHUCHUCNANG.MALOAIKHUCN=DM_LOAIKHUCHUCNANG.ID and KHUCHUCNANG.captinh = 0
 left join khuchucnang_mdsd on DM_MUCDICHSUDUNG.ID=khuchucnang_mdsd.ID_MDSD  and khuchucnang_mdsd.id_khucn=  KHUCHUCNANG.ID

 where  dm_mucdichsudung.STT is not null  and (ID_KYQH=IDKYQH or ID_KYQH is null) and (khuchucnang.mahuyen=IDHUYEN or khuchucnang.mahuyen is null)
group by dm_loaikhuchucnang.kyhieu,dm_mucdichsudung.KIHIEU,dm_mucdichsudung.STT, khuchucnang.mahuyen
order by dm_mucdichsudung.STT;

Exception
When No_Data_Found Then
     -- Tr??ng h?p câu SELECT không tr? v? b?n ghi nào
     Dbms_Output.Put_Line('No data with emp_id= ');
END;




