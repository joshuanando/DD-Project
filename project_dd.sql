DROP TABLE TOOLS CASCADE CONSTRAINTS;
DROP TABLE SPAREPART CASCADE CONSTRAINTS;
DROP TABLE TOOLS_CATEGORY CASCADE CONSTRAINTS;
DROP TABLE SPAREPART_CATEGORY CASCADE CONSTRAINTS;
DROP TABLE DTRANS CASCADE CONSTRAINTS;
DROP TABLE HTRANS CASCADE CONSTRAINTS;
DROP TABLE JASA CASCADE CONSTRAINTS;
DROP MATERIALIZED VIEW TOOLS_cabdave;
DROP MATERIALIZED VIEW TOOLS_cabjon;
DROP MATERIALIZED VIEW TOOLS_cabbry;
DROP MATERIALIZED VIEW TOOLS_cabnando;
DROP MATERIALIZED VIEW SPAREPART_cabdave;
DROP MATERIALIZED VIEW SPAREPART_cabjon;
DROP MATERIALIZED VIEW SPAREPART_cabbry;
DROP MATERIALIZED VIEW SPAREPART_cabnando;
DROP MATERIALIZED VIEW LOG ON TOOLS;
DROP MATERIALIZED VIEW LOG ON SPAREPART;
DROP MATERIALIZED VIEW LOG ON TOOLS_CATEGORY;
DROP MATERIALIZED VIEW LOG ON SPAREPART_CATEGORY;
DROP PUBLIC DATABASE LINK CABDAVE;
DROP PUBLIC DATABASE LINK CABJON;
DROP PUBLIC DATABASE LINK CABBRY;
DROP PUBLIC DATABASE LINK CABNANDO;
UNDEFINE LOCALID;
PURGE RECYCLEBIN;
SET SERVEROUTPUT ON;

-- INI DIGANTI STRING IDENTIFIER CABANG SENDIRI SEBELUM DI RUN DI SQLPLUS
DEF LOCALID = 'CBB'
select '&LOCALID' from dual;

CREATE TABLE JASA(
	ID_JASA VARCHAR2(10) PRIMARY KEY,
	NAMA VARCHAR2(100),
	HARGA NUMBER,
	STATUS NUMBER(1)
);

CREATE TABLE TOOLS_CATEGORY(
	ID_CATEGORY VARCHAR2(10) PRIMARY KEY,
	-- CAT/001
	NAMA VARCHAR2(100)
);

CREATE TABLE TOOLS(
	ID_TOOLS VARCHAR2(10) PRIMARY KEY,
	-- AHASS/001
	NAMA VARCHAR2(100),
	ID_CATEGORY VARCHAR2(10),
	STATUS NUMBER(1),
	-- 0 = not available, 1 = available
	FOREIGN KEY (ID_CATEGORY) REFERENCES TOOLS_CATEGORY(ID_CATEGORY) 
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
	HARGA NUMBER DEFAULT 0,	
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
	DARI_CABANG VARCHAR2(10) DEFAULT '-',
	STATUS NUMBER(1) DEFAULT 0,
	pegawai VARCHAR2(100) DEFAULT ''	
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

create table history_stok(
    id varchar2(10),
    nama_barang varchar2(20),
    stok_sebelum number(3),
    stok_sesudah number(3),
    keterangan varchar2(30),
    tanggal date
);

--------------------TRIGGERS--------------------------
--ID JASA
CREATE OR REPLACE TRIGGER create_id_jasa 
BEFORE INSERT ON JASA
FOR EACH ROW
DECLARE
	IDLAMA VARCHAR2(10);
	countid NUMBER(10);
	localid VARCHAR2(10);
BEGIN
	select MAX(ID_JASA) into IDLAMA from JASA;
	select '&LOCALID' into localid from dual;
	if (IDLAMA IS NULL) then 
		countid := 1;
	else 
		countid := substr(IDLAMA,-3,3)+1;
	end if;
	:NEW.ID_JASA :=  localid || '/J' || lpad(countid,3,'0');
END;
/
SHOW ERR;

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
	select max(ID_CATEGORY) into temp_id from TOOLS_CATEGORY;
	if temp_id IS NULL then
		temp_id:=1;
	ELSE 
	temp_id := substr(temp_id,-3,3)+1;
	end if;

	:new.ID_CATEGORY := 'CAT'||lpad(temp_id,3,'0');
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

--ID DTRANS
Create or replace Trigger autoIdDtrans
before insert 
    on DTRANS
    for each row
declare 
    temp_id varchar2(10);
    localid varchar2(10);
    err exception;
begin
	select max(id) into temp_id from DTRANS;	
	select '&LOCALID' into localid from dual;	

	if temp_id IS NULL then
		temp_id:=1;
	ELSE 
		temp_id := substr(temp_id,-3,3)+1;
	end if;

	:new.id := localid||'/DT'||lpad(temp_id,3,'0');
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
Create or replace Trigger autoSumHtrans
for insert or update or delete on dtrans 
compound trigger
    tot number;
    err exception;
	tempid varchar2(10);
BEFORE EACH ROW IS
BEGIN
	if updating or inserting then
		tempid := :NEW.ID_HTRANS;
	end if;
	if deleting then
		tempid := :OLD.ID_HTRANS;	
	end if;
END BEFORE EACH ROW;
after statement is 
begin
	select to_number(SUM(HARGA_ITEM*JUMLAH)) into TOT from DTRANS WHERE ID_HTRANS=tempid;
	UPDATE HTRANS SET TOTAL=to_number(tot) where ID_TRANSAKSI=tempid; 
exception 
    when err then raise_application_error(-20006,'err sumhtrans');	
end after statement;
END autoSumHtrans;
/
show err;
COMMIT;

--id history stok
Create or replace Trigger autoIdHistoryStok
before insert 
    on history_stok
    for each row
declare 
    temp_id varchar2(10);
	localid VARCHAR2(10);
    err exception;
begin
	select max(id) into temp_id from history_stok;
	select '&LOCALID' into localid from dual;	
	
	if temp_id IS NULL then
		temp_id:=1;
	ELSE 
	temp_id := substr(temp_id,-3,3)+1;
	end if;
	:new.id := localid || '/H' ||lpad(temp_id,3,'0');
exception 
    when err then raise_application_error(-20008,'hangus');
END;
/
show err;

--trigger update history
Create or replace Trigger updateStokHistory
after update 
    on sparepart
    for each row
declare 
    temp_new number(3);
    temp_old number(3);
    err exception;
begin
    temp_new := :new.stok;
    temp_old := :old.stok;
    if( temp_new > temp_old) then
        insert into history_stok values('',:new.name,:old.stok ,:new.stok,'restok barang', current_date);  
    else
        insert into history_stok values('',:new.name,:old.stok ,:new.stok,'barang terpakai/terjual', current_date);
    end if;
    
exception 
    when err then raise_application_error(-20009,'hangus');
END;
/
show err;


----------------------------------------------

-- AHASS/001

-- INSERT INTO TOOLS_CATEGORY VALUES ('','PUMP');
-- INSERT INTO TOOLS VALUES ('','PUMP MOTOR YAMAHA','CAT001',1);
-- INSERT INTO TOOLS VALUES ('','PUMP MOTOR HARLEY','CAT001',0);
-- INSERT INTO SPAREPART_CATEGORY VALUES ('','SPION');
-- INSERT INTO SPAREPART VALUES ('','Spion Honda','SC001',0,'Spion dari honda');

--------------------Database Link--------------------------
-- comment yang punya sendiri
-- field 1 = username
-- field 2 = password 
-- field 3 = tns listener name
CREATE PUBLIC DATABASE LINK cabdave CONNECT TO admin IDENTIFIED BY admin USING 'cabdave';
CREATE PUBLIC DATABASE LINK cabjon CONNECT TO admin IDENTIFIED BY admin USING 'cabjon'; 
CREATE PUBLIC DATABASE LINK cabnando CONNECT TO admin IDENTIFIED BY nando USING 'cabnando'; 
--CREATE PUBLIC DATABASE LINK cabbry CONNECT TO admin IDENTIFIED BY admin USING 'cabbry';

----------------------------VIEW---------------------------------

CREATE or Replace VIEW ITEMS as 
select j.id_jasa as "ID", j.nama as "NAMA", j.harga as "HARGA", 'JASA' as "KATEGORI", 
(CASE WHEN j.STATUS = 1 THEN 'Available' ELSE 'Not Available' END) as "STATUS" 
from jasa j
union
(select t.id_tools as "ID" ,t.nama as "NAMA", 0 as HARGA, tc.nama as "KATEGORI", 
(CASE WHEN T.STATUS = 1 THEN 'Available' ELSE 'Not Available' END) as "STATUS"  
from tools t,tools_category tc 
where t.ID_CATEGORY = tc.ID_CATEGORY
union
select s.id_spare as "ID" ,s.name as "NAMA",s.harga as "HARGA" ,sc.category_name as "KATEGORI",
 '@'||s.stok as "STATUS"  
from sparepart s,sparepart_category sc 
where s.id_category = sc.id_category);

-- KASIR

DROP ROLE KASIR;
CREATE ROLE KASIR;

GRANT CREATE SESSION TO KASIR;
GRANT SELECT, UPDATE, INSERT, DELETE ON TOOLS TO KASIR;
GRANT SELECT, UPDATE, INSERT, DELETE ON SPAREPART TO KASIR;
GRANT SELECT, UPDATE, INSERT, DELETE ON HTRANS TO KASIR;
GRANT SELECT, UPDATE, INSERT, DELETE ON DTRANS TO KASIR;
GRANT SELECT ON SPAREPART_CATEGORY TO KASIR;
GRANT SELECT ON TOOLS_CATEGORY TO KASIR;
GRANT SELECT ON CABANG TO KASIR;
GRANT SELECT ON JASA TO KASIR;
GRANT SELECT ON ITEMS TO KASIR;

--------------------MATERIALIZED VIEW--------------------------

CREATE MATERIALIZED VIEW LOG ON TOOLS with ROWID;
CREATE MATERIALIZED VIEW LOG ON TOOLS_CATEGORY with ROWID;

CREATE MATERIALIZED VIEW TOOLS_cabdave as select T.ID_TOOLS,T.NAMA,TC.NAMA as Kategori, (CASE WHEN T.STATUS = 1 THEN 'Available' ELSE 'Not Available' END) as Status FROM TOOLS@cabdave T,TOOLS_CATEGORY@cabdave TC WHERE T.ID_CATEGORY=TC.ID_CATEGORY;
CREATE MATERIALIZED VIEW TOOLS_cabjon as select T.ID_TOOLS,T.NAMA,TC.NAMA as Kategori, (CASE WHEN T.STATUS = 1 THEN 'Available' ELSE 'Not Available' END) as Status FROM TOOLS@cabjon T,TOOLS_CATEGORY@cabjon TC WHERE T.ID_CATEGORY=TC.ID_CATEGORY;
CREATE MATERIALIZED VIEW TOOLS_cabbry as select T.ID_TOOLS,T.NAMA,TC.NAMA as Kategori, (CASE WHEN T.STATUS = 1 THEN 'Available' ELSE 'Not Available' END) as Status FROM TOOLS@cabbry T,TOOLS_CATEGORY@cabbry TC WHERE T.ID_CATEGORY=TC.ID_CATEGORY;
CREATE MATERIALIZED VIEW TOOLS_cabnando as select T.ID_TOOLS,T.NAMA,TC.NAMA as Kategori, (CASE WHEN T.STATUS = 1 THEN 'Available' ELSE 'Not Available' END) as Status FROM TOOLS@cabnando T,TOOLS_CATEGORY@cabnando TC WHERE T.ID_CATEGORY=TC.ID_CATEGORY;

CREATE MATERIALIZED VIEW LOG ON SPAREPART with ROWID;
CREATE MATERIALIZED VIEW LOG ON SPAREPART_CATEGORY with ROWID;

CREATE MATERIALIZED VIEW SPAREPART_cabdave as select S.ID_SPARE,S.NAME,SC.CATEGORY_NAME,S.STOK,S.HARGA,S.DESCRIPTION FROM SPAREPART@cabdave S, SPAREPART_CATEGORY@cabdave SC WHERE S.ID_CATEGORY = SC.ID_CATEGORY; 
CREATE MATERIALIZED VIEW SPAREPART_cabjon as select S.ID_SPARE,S.NAME,SC.CATEGORY_NAME,S.STOK,S.HARGA,S.DESCRIPTION FROM SPAREPART@cabjon S, SPAREPART_CATEGORY@cabjon SC WHERE S.ID_CATEGORY = SC.ID_CATEGORY; 
CREATE MATERIALIZED VIEW SPAREPART_cabbry as select S.ID_SPARE,S.NAME,SC.CATEGORY_NAME,S.STOK,S.HARGA,S.DESCRIPTION FROM SPAREPART@cabbry S, SPAREPART_CATEGORY@cabbry SC WHERE S.ID_CATEGORY = SC.ID_CATEGORY; 
CREATE MATERIALIZED VIEW SPAREPART_cabnando as select S.ID_SPARE,S.NAME,SC.CATEGORY_NAME,S.STOK,S.HARGA,S.DESCRIPTION FROM SPAREPART@cabnando S, SPAREPART_CATEGORY@cabnando SC WHERE S.ID_CATEGORY = SC.ID_CATEGORY; 


DROP MATERIALIZED VIEW ITEMS_cabdave;
DROP MATERIALIZED VIEW ITEMS_cabjon;
DROP MATERIALIZED VIEW ITEMS_cabbry;
DROP MATERIALIZED VIEW ITEMS_cabnando;

CREATE MATERIALIZED VIEW ITEMS_cabdave as select * FROM ADMIN.ITEMS@cabdave;
CREATE MATERIALIZED VIEW ITEMS_cabjon as select * FROM ADMIN.ITEMS@cabjon;
CREATE MATERIALIZED VIEW ITEMS_cabbry as select * FROM ADMIN.ITEMS@cabbry;
CREATE MATERIALIZED VIEW ITEMS_cabnando as select * FROM ADMIN.ITEMS@cabnando;


----------- INSERT AFTER CREATING ALL MVIEW --------------------
--------------------PROCEDURE-----------------------------------

--Procedure insert sparepart category
CREATE OR REPLACE PROCEDURE Insert_Cat_Sparepart ( name IN VARCHAR2)
IS
BEGIN
	INSERT INTO SPAREPART_CATEGORY@cabdave VALUES ('', name);
	INSERT INTO SPAREPART_CATEGORY@cabjon VALUES ('', name);
	INSERT INTO SPAREPART_CATEGORY@cabbry VALUES ('', name);
	INSERT INTO SPAREPART_CATEGORY@cabnando VALUES ('', name);
	commit;
END;
/
show err;

CREATE OR REPLACE PROCEDURE Update_Cat_Sparepart ( name IN VARCHAR2, id in VARCHAR2)
IS
BEGIN
	UPDATE SPAREPART_CATEGORY@cabdave SET CATEGORY_NAME = name WHERE ID_CATEGORY = id;
	UPDATE SPAREPART_CATEGORY@cabjon SET CATEGORY_NAME = name WHERE ID_CATEGORY = id;
	UPDATE SPAREPART_CATEGORY@cabbry SET CATEGORY_NAME = name WHERE ID_CATEGORY = id;
	UPDATE SPAREPART_CATEGORY@cabnando SET CATEGORY_NAME = name WHERE ID_CATEGORY = id;	
	commit;
END;
/
show err;

CREATE OR REPLACE PROCEDURE Delete_Cat_Sparepart ( id in VARCHAR2)
IS
BEGIN
	DELETE FROM SPAREPART_CATEGORY@cabdave WHERE ID_CATEGORY = id;
	DELETE FROM SPAREPART_CATEGORY@cabjon WHERE ID_CATEGORY = id;
	DELETE FROM SPAREPART_CATEGORY@cabbry WHERE ID_CATEGORY = id;
	DELETE FROM SPAREPART_CATEGORY@cabnando WHERE ID_CATEGORY = id;
	commit;
END;
/
show err;

CREATE OR REPLACE PROCEDURE Insert_Cat_Tools ( name IN VARCHAR2)
IS
BEGIN
	INSERT INTO TOOLS_CATEGORY@cabdave VALUES ('', name);
	INSERT INTO TOOLS_CATEGORY@cabjon VALUES ('', name);
	INSERT INTO TOOLS_CATEGORY@cabbry VALUES ('', name);
	INSERT INTO TOOLS_CATEGORY@cabnando VALUES ('', name);
	commit;
END;
/
show err;

CREATE OR REPLACE PROCEDURE Update_Cat_Tools ( name IN VARCHAR2, id in VARCHAR2)
IS
BEGIN
	UPDATE TOOLS_CATEGORY@cabdave SET NAMA = name WHERE ID_CATEGORY = id;
	UPDATE TOOLS_CATEGORY@cabjon SET NAMA = name WHERE ID_CATEGORY = id;
	UPDATE TOOLS_CATEGORY@cabbry SET NAMA = name WHERE ID_CATEGORY = id;
	UPDATE TOOLS_CATEGORY@cabnando SET NAMA = name WHERE ID_CATEGORY = id;	
	commit;
END;
/
show err;

CREATE OR REPLACE PROCEDURE Delete_Cat_Tools ( id in VARCHAR2)
IS
BEGIN

	DELETE FROM TOOLS_CATEGORY@cabdave WHERE ID_CATEGORY = id;
	DELETE FROM TOOLS_CATEGORY@cabjon WHERE ID_CATEGORY = id;
	DELETE FROM TOOLS_CATEGORY@cabbry WHERE ID_CATEGORY = id;
	DELETE FROM TOOLS_CATEGORY@cabnando WHERE ID_CATEGORY = id;
	commit;
END;
/
show err;

CREATE OR REPLACE PROCEDURE CREATE_PEGAWAI (username IN VARCHAR2, password in VARCHAR2 , cabang in VARCHAR2)
IS
BEGIN
	EXECUTE IMMEDIATE 'DROP USER' || username;
	EXECUTE IMMEDIATE 'CREATE USER' || username || 'IDENTIFIED BY' || password;
	EXECUTE IMMEDIATE 'GRANT KASIR TO' || username;
END;
/
show err;

CREATE OR REPLACE PROCEDURE CHANGE_PASS_PEGAWAI (username IN VARCHAR2, password in VARCHAR2 , cabang in VARCHAR2)
IS
BEGIN
	EXECUTE IMMEDIATE 'ALTER USER' || username || 'IDENTIFIED BY' || password;
END;
/
show err;

CREATE OR REPLACE PROCEDURE DELETE_PEGAWAI (username IN VARCHAR2 , cabang in VARCHAR2)
IS
BEGIN
	EXECUTE IMMEDIATE 'DROP USER' || username;
END;
/
show err;

CREATE OR REPLACE PROCEDURE refresh ( name IN VARCHAR2)
IS
BEGIN
	DBMS_MVIEW.REFRESH(name);
    DBMS_OUTPUT.PUT_LINE('REFRESH '|| name ||' SUCCESS!');
EXCEPTION
    WHEN OTHERS THEN
		DBMS_OUTPUT.PUT_LINE('REFRESH '|| name ||' FAILED!');
        NULL;
END;
/
show err;

CREATE OR REPLACE PROCEDURE refresh_all 
IS
BEGIN
	refresh('tools_cabdave');
	refresh('tools_cabjon');
	refresh('tools_cabnando');
    refresh('tools_cabbry');
    refresh('sparepart_cabdave');
    refresh('sparepart_cabjon');
    refresh('sparepart_cabnando');
    refresh('sparepart_cabbry');	
    refresh('items_cabdave');
    refresh('items_cabjon');
    refresh('items_cabnando');
    refresh('items_cabbry');	
END;
/
show err;

-- buat mendapatkan next htrans di kasir
CREATE OR REPLACE FUNCTION nextHtransId
RETURN VARCHAR2
IS
    temp_id varchar2(10);
    localid varchar2(10);
BEGIN	
	select max(ID_Transaksi) into temp_id from HTRANS;	
	select '&LOCALID' into localid from dual;	
	if temp_id IS NULL then
		temp_id:=1;
	ELSE 
	temp_id := substr(temp_id,-3,3)+1;
	end if;
	RETURN localid||'/HT'||lpad(temp_id,3,'0');
END nextHtransId;
/
show err;

GRANT EXECUTE ON nextHtransId TO KASIR;
GRANT EXECUTE ON REFRESH_ALL TO KASIR;
GRANT EXECUTE ON REFRESH TO KASIR;

GRANT SELECT ON SPAREPART_CABBRY TO KASIR;
GRANT SELECT ON SPAREPART_CABJON TO KASIR;
GRANT SELECT ON SPAREPART_CABDAVE TO KASIR;
GRANT SELECT ON SPAREPART_CABNANDO TO KASIR;
GRANT SELECT ON TOOLS_CABBRY TO KASIR;
GRANT SELECT ON TOOLS_CABJON TO KASIR;
GRANT SELECT ON TOOLS_CABDAVE TO KASIR;
GRANT SELECT ON TOOLS_CABNANDO TO KASIR;

GRANT SELECT ON ITEMS_CABBRY TO KASIR;
GRANT SELECT ON ITEMS_CABJON TO KASIR;
GRANT SELECT ON ITEMS_CABDAVE TO KASIR;
GRANT SELECT ON ITEMS_CABNANDO TO KASIR;

DROP USER KASIR1;
CREATE USER KASIR1 IDENTIFIED BY kasir1;
GRANT KASIR TO KASIR1;

-- Dbms_Scheduler.Drop_Job (Job_Name => 'REFRESH')

-- BEGIN
--  DBMS_SCHEDULER.CREATE_JOB(
--  JOB_NAME => 'SCHEDULER 1',  
--   --KALO PAKE PROCEDURE
--  JOB_TYPE => 'STORED PROCEDURE',
--  JOB_ACTION => 'REFRESH_MV',      -- INI NAMA PROCEDURE E 
--   --KALO PAKE PLSQ LANGSUNG
--  --JOB_TYPE => 'PLSQL_BLOCK',
--  -- JOB_ACTION =>     -- INI CODE PLSQL
--   -- 'BEGIN
--    -- EXECUTE DBMS_MVIEW.REFRESH('');
--    -- EXECUTE DBMS_MVIEW.REFRESH('');
--    -- EXECUTE DBMS_MVIEW.REFRESH('');
--    -- EXECUTE DBMS_MVIEW.REFRESH('');
--    -- EXECUTE DBMS_MVIEW.REFRESH('');
--    -- EXECUTE DBMS_MVIEW.REFRESH('');
--    -- EXECUTE DBMS_MVIEW.REFRESH('');
--    -- EXECUTE DBMS_MVIEW.REFRESH('');
--    -- COMMIT;
--   -- END;',     
--  REPEAT_INTERVAL => 'FREQ=DAILY; BYHOUR=6,18,23 ;BYMINUTE=00',
--  ENABLED => 'TRUE'
-- );
-- END;
-- /

EXECUTE DBMS_SCHEDULER.DROP_JOB(Job_Name => 'REFRESH_ALL_MV');
BEGIN
 DBMS_SCHEDULER.CREATE_JOB(
	JOB_NAME => 'REFRESH_ALL_MV',
	JOB_TYPE => 'STORED_PROCEDURE',
	JOB_ACTION => 'REFRESH_ALL', 
	REPEAT_INTERVAL => 'FREQ=DAILY; BYHOUR=6,18,23 ;BYMINUTE=00;',
	ENABLED => TRUE
 );
END;
/