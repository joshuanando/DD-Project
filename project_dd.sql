DROP TABLE TOOLS CASCADE CONSTRAINTS;
DROP TABLE SPAREPART CASCADE CONSTRAINTS;
DROP TABLE TOOLS_CATEGORY CASCADE CONSTRAINTS;
DROP TABLE SPAREPART_CATEGORY CASCADE CONSTRAINTS;
DROP TABLE DTRANS CASCADE CONSTRAINTS;
DROP TABLE HTRANS CASCADE CONSTRAINTS;
DROP MATERIALIZED VIEW LOCAL_TOOLS;
DROP MATERIALIZED VIEW LOCAL_SPAREPART;
DROP PUBLIC DATABASE LINK CABDAVE;
DROP PUBLIC DATABASE LINK CABJON;
DROP PUBLIC DATABASE LINK CABBRY;
DROP PUBLIC DATABASE LINK CABNANDO;
UNDEFINE LOCALID;
PURGE RECYCLEBIN;

-- INI DIGANTI STRING IDENTIFIER CABANG SENDIRI SEBELUM DI RUN DI SQLPLUS
DEF LOCALID = 'CBJ'
select '&LOCALID' from dual;

CREATE TABLE TOOLS_CATEGORY(
	ID_KATEGORI VARCHAR2(10) PRIMARY KEY,
	-- CAT/001
	NAMA VARCHAR2(100)
);

CREATE TABLE TOOLS(
	ID_TOOLS VARCHAR2(10) PRIMARY KEY,
	-- AHASS/001
	NAMA VARCHAR2(100),
	ID_KATEGORI VARCHAR2(10),
	STATUS NUMBER(1),
	-- 0 = not available, 1 = available
	FOREIGN KEY (ID_KATEGORI) REFERENCES TOOLS_CATEGORY(ID_KATEGORI) 
);

CREATE TABLE SPAREPART_CATEGORY(
	ID_CATEGORY VARCHAR2(10) PRIMARY KEY,
	CATEGORY_NAME VARCHAR2(100)
);

CREATE TABLE SPAREPART(
	ID_SPARE VARCHAR2(10) PRIMARY KEY,
	NAME VARCHAR2(100) NOT NULL,
	ID_CATEGORY VARCHAR2(20),
	STOK NUMBER(10) DEFAULT 0,
	DESCRIPTION VARCHAR2(100),
	FOREIGN KEY (ID_CATEGORY) REFERENCES SPAREPART_CATEGORY(ID_CATEGORY)
);

CREATE TABLE HTRANS (
	ID_Transaksi VARCHAR(14) PRIMARY KEY,
	Tanggal DATE,
	NAMA_PEMILIK VARCHAR2(50),
	ALAMAT_PEMILIK VARCHAR2(100),
	NO_KTP VARCHAR2(18),
	NPWP VARCHAR2(20),
	NO_POLISI VARCHAR2(20),
	DESKRIPSI_KENDARAAN VARCHAR2(100),
	TOTAL NUMBER,
	STATUS NUMBER(1) DEFAULT 0
);

CREATE TABLE DTRANS (
	id VARCHAR2(10) PRIMARY KEY,
	ID_HTRANS VARCHAR2(14),
	ID_ITEM VARCHAR2(10),
	NAMA_ITEM VARCHAR2(100),
	HARGA_ITEM NUMBER,
	JUMLAH NUMBER,
	FOREIGN KEY (ID_HTRANS) REFERENCES HTRANS(ID_Transaksi)
);

--------------------TRIGGERS--------------------------
--ID TOOLS
CREATE OR REPLACE TRIGGER create_id_tools 
BEFORE INSERT ON TOOLS
FOR EACH ROW
DECLARE
	IDLAMA VARCHAR2(10);
	countid NUMBER(10);
	localid VARCHAR2(10);
BEGIN
	select MAX(ID_TOOLS) into IDLAMA from TOOLS;
	select '&LOCALID' into localid from dual;
	if (IDLAMA IS NULL) then 
		countid := 1;
	else 
		countid := substr(IDLAMA,-3,3)+1;
	end if;
	:NEW.ID_TOOLS :=  localid || '/T' || lpad(countid,3,'0');
END;
/
SHOW ERR;

--ID TOOLS_CATEGORY
Create or replace Trigger autoIdToolsCategory
before insert 
    on TOOLS_CATEGORY
    for each row
declare 
    temp_id varchar2(10);
    err exception;
begin
	select max(ID_KATEGORI) into temp_id from TOOLS_CATEGORY;
	if temp_id IS NULL then
		temp_id:=1;
	ELSE 
	temp_id := substr(temp_id,-3,3)+1;
	end if;

	:new.ID_KATEGORI := 'CAT'||lpad(temp_id,3,'0');
exception 
    when err then raise_application_error(-20006,'hangus');
END;
/
show err;

--ID sparepart
Create or replace Trigger autoIdSparepart 
before insert 
    on sparepart
    for each row
declare 
    temp_id varchar2(10);
	localid VARCHAR2(10);
    err exception;
begin
	select max(id_spare) into temp_id from sparepart;
	select '&LOCALID' into localid from dual;	
	
	if temp_id IS NULL then
		temp_id:=1;
	ELSE 
	temp_id := substr(temp_id,-3,3)+1;
	end if;
	:new.id_spare := localid || '/S' ||lpad(temp_id,3,'0');
exception 
    when err then raise_application_error(-20001,'hangus');
END;
/
show err;

--ID sparepartCategory
Create or replace Trigger autoIdSparepartCategory 
before insert 
    on SPAREPART_CATEGORY
    for each row
declare 
    temp_id varchar2(10);
    err exception;
begin
	select max(ID_CATEGORY) into temp_id from SPAREPART_CATEGORY;		
	if temp_id IS NULL then
		temp_id:=1;
	ELSE 
	temp_id := substr(temp_id,-3,3)+1;
	end if;
	:new.ID_CATEGORY := 'SC'||lpad(temp_id,3,'0');
exception 
    when err then raise_application_error(-20002,'hangus');
END;
/
show err;

--ID STOK_SPAREPART
/* Create or replace Trigger autoIdStokSparepart
before insert 
    on STOK_SPAREPART
    for each row
declare 
    temp_id varchar2(10);
    err exception;
begin
	select max(ID) into temp_id from STOK_SPAREPART;

	if temp_id IS NULL then
		temp_id:=1;
	ELSE 
	temp_id := temp_id+1;
	end if;

	:new.ID := temp_id;
exception 
    when err then raise_application_error(-20003,'hangus');
END;
/
show err; */

--ID HTRANS
Create or replace Trigger autoIdHtrans
before insert 
    on HTRANS
    for each row
declare 
    temp_id varchar2(10);
    localid varchar2(10);
    err exception;
begin
	select max(ID_Transaksi) into temp_id from HTRANS;	
	select '&LOCALID' into localid from dual;	

	if temp_id IS NULL then
		temp_id:=1;
	ELSE 
	temp_id := substr(temp_id,-3,3)+1;
	end if;

	:new.ID_Transaksi := localid||'/HT'||lpad(temp_id,3,'0');
exception 
    when err then raise_application_error(-20004,'hangus');
END;
/
show err;

--ID CABANG
Create or replace Trigger autoIdCabang
before insert 
    on CABANG
    for each row
declare 
    temp_id varchar2(10);
    localid varchar2(10);
    err exception;
begin
	select max(ID_CABANG) into temp_id from CABANG;
	select '&LOCALID' into localid from dual;	

	if temp_id IS NULL then
		temp_id:=1;
	ELSE 
	temp_id := substr(temp_id,-3,3)+1;
	end if;

	:new.ID_CABANG := localid||'/C'||lpad(temp_id,3,'0');
exception 
    when err then raise_application_error(-20005,'hangus');
END;
/
show err;


--TRIGGER UPDATE HTRANS
Create or replace Trigger autoSumHtrans3
after insert 
    on dtrans
    for each row
declare 
    tot number(11);
    err exception;
begin
	select SUM(HARGA_ITEM*JUMLAH) into TOT from DTRANS WHERE ID_HTRANS=:NEW.ID_HTRANS;
	UPDATE HTRANS SET TOTAL=tot where ID_TRANSAKSI=:NEW.ID_HTRANS;
exception 
    when err then raise_application_error(-20009,'err sumhtrans');
END;
/
show err;

Create or replace Trigger autoSumHtrans1
after UPDATE 
    on dtrans
    for each row
declare 
    tot number(11);
    err exception;
begin
	select SUM(HARGA_ITEM*JUMLAH) into TOT from DTRANS WHERE ID_HTRANS=:NEW.ID_HTRANS;
	UPDATE HTRANS SET TOTAL=tot where ID_TRANSAKSI=:NEW.ID_HTRANS;
exception 
    when err then raise_application_error(-20007,'err sumhtrans');
END;
/
show err;

Create or replace Trigger autoSumHtrans2
after delete 
    on dtrans
    for each row
declare 
    tot number;
    err exception;
begin
	select SUM(HARGA_ITEM*JUMLAH) into TOT from DTRANS WHERE ID_HTRANS=:NEW.ID_HTRANS;
	UPDATE HTRANS SET TOTAL=tot where ID_TRANSAKSI=:NEW.ID_HTRANS;
exception 
    when err then raise_application_error(-20008,'err sumhtrans');
END;
/
show err;
COMMIT;

----------------------------------------------

-- AHASS/001
CREATE MATERIALIZED VIEW LOCAL_TOOLS as select T.ID_TOOLS,T.NAMA,TC.NAMA as Kategori, (CASE WHEN T.STATUS = 1 THEN 'Available' ELSE 'Not Available' END) as Status FROM TOOLS T,TOOLS_CATEGORY TC WHERE T.ID_KATEGORI=TC.ID_KATEGORI;
CREATE MATERIALIZED VIEW LOCAL_SPAREPART as select S.ID_SPARE,S.NAME,SC.CATEGORY_NAME,S.STOK,S.DESCRIPTION FROM SPAREPART S, SPAREPART_CATEGORY SC WHERE S.ID_CATEGORY = SC.ID_CATEGORY; 

INSERT INTO TOOLS_CATEGORY VALUES ('','PUMP');
INSERT INTO TOOLS VALUES ('','PUMP MOTOR YAMAHA','CAT001',1);
-- INSERT INTO TOOLS VALUES ('','PUMP MOTOR HARLEY','CAT001',0);
-- INSERT INTO SPAREPART_CATEGORY VALUES ('','SPION');
-- INSERT INTO SPAREPART VALUES ('','Spion Honda','SC001',0,'Spion dari honda');


-- comment yang punya sendiri
-- field 1 = username
-- field 2 = password 
-- field 3 = tns listener name
CREATE PUBLIC DATABASE LINK cabdave CONNECT TO admin IDENTIFIED BY admin USING 'cabdave';
CREATE PUBLIC DATABASE LINK cabjon CONNECT TO admin IDENTIFIED BY admin USING 'cabjon'; 
CREATE PUBLIC DATABASE LINK cabnando CONNECT TO admin IDENTIFIED BY nando USING 'cabnando'; 
CREATE PUBLIC DATABASE LINK cabbry CONNECT TO admin IDENTIFIED BY admin USING 'cabbry'; 

-- KASIR
DROP USER KASIR;
CREATE USER KASIR IDENTIFIED BY kasir;
GRANT CREATE SESSION TO KASIR;
GRANT SELECT, UPDATE, INSERT, DELETE ON TOOLS TO KASIR;
GRANT SELECT, UPDATE, INSERT, DELETE ON SPAREPART TO KASIR;
GRANT SELECT, UPDATE, INSERT, DELETE ON HTRANS TO KASIR;
GRANT SELECT, UPDATE, INSERT, DELETE ON DTRANS TO KASIR;
GRANT SELECT ON SPAREPART_CATEGORY TO KASIR;
GRANT SELECT ON TOOLS_CATEGORY TO KASIR;
GRANT SELECT ON CABANG TO KASIR;
-- select * from admin.tools;

-- VIEW ITEMS
CREATE or Replace VIEW ITEMS as 
select t.id_tools as "ID" ,t.nama as "NAMA",tc.nama as "KATEGORI", 
(CASE WHEN T.STATUS = 1 THEN 'Available' ELSE 'Not Available' END) as "STATUS"  
from tools t,tools_category tc 
where t.id_kategori = tc.id_kategori
union
select s.id_spare as "ID" ,s.name as "NAMA",sc.category_name as "KATEGORI", '@'||s.stok as "STATUS"  
from sparepart s,sparepart_category sc 
where s.id_category = sc.id_category;
GRANT SELECT ON ITEMS TO KASIR;
