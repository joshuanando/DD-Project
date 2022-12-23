SET LINESIZE 2000;
-- INSERT JASA
INSERT INTO JASA VALUES('','SERVIS RINGAN',70000,1);
INSERT INTO JASA VALUES('','SERVIS PERBARUAN MEDIUM',120000,1);
INSERT INTO JASA VALUES('','SERVIS FULL',200000,1);

--INSERT TOOLS CAT
INSERT INTO TOOLS_CATEGORY VALUES ('','COMMON TOOLS');
INSERT INTO TOOLS_CATEGORY VALUES ('','ENGINE SPECIAL TOOLS');
INSERT INTO TOOLS_CATEGORY VALUES ('','FRAME SPECIAL TOOLS');
INSERT INTO TOOLS_CATEGORY VALUES ('','ELECTIC SPECIAL TOOLS');
INSERT INTO TOOLS_CATEGORY VALUES ('','MEASUREMENT TOOLS');
INSERT INTO TOOLS_CATEGORY VALUES ('','STRATEGIC TOOLS');

--INSERT TOOLS
INSERT INTO TOOLS VALUES ('','KUNCI SOK / SOCKET SET', 'CAT001',1);
INSERT INTO TOOLS VALUES ('','KUNCI "L" HEXAGONAL', 'CAT001',1);
INSERT INTO TOOLS VALUES ('','KUNCI "L" TORX', 'CAT001',1);
INSERT INTO TOOLS VALUES ('','IMPACT SCREWDRIVER', 'CAT001',1);
INSERT INTO TOOLS VALUES ('','PNEUMATIC IMPACT WRENCH', 'CAT001',1);

INSERT INTO TOOLS VALUES ('','Flywheel Puller', 'CAT002',1);
INSERT INTO TOOLS VALUES ('','Flywheel Holder', 'CAT002',1);
INSERT INTO TOOLS VALUES ('','Gear Holder', 'CAT002',1);
INSERT INTO TOOLS VALUES ('','Valve Spring Compressor', 'CAT002',1);
INSERT INTO TOOLS VALUES ('','Lock Nut Wrench', 'CAT002',1);

INSERT INTO TOOLS VALUES ('','Spoke Wrench', 'CAT003',1);
INSERT INTO TOOLS VALUES ('','Wheel Truing', 'CAT003',1);

INSERT INTO TOOLS VALUES ('','DLC Short Connector', 'CAT004',1);
INSERT INTO TOOLS VALUES ('','PIN Probe Male', 'CAT004',1);
INSERT INTO TOOLS VALUES ('','HDS(Honda Diagnosa System)', 'CAT004',1);
INSERT INTO TOOLS VALUES ('','Jumpel Line', 'CAT004',1);

INSERT INTO TOOLS VALUES ('','Feeler Gauge / Thickness Gauge', 'CAT005',1);
INSERT INTO TOOLS VALUES ('','Mistar Baja', 'CAT005',1);
INSERT INTO TOOLS VALUES ('','Jangka Sorong / Caliper', 'CAT005',1);
INSERT INTO TOOLS VALUES ('','Outside Micrometer', 'CAT005',1);
INSERT INTO TOOLS VALUES ('','Inside Micrometer', 'CAT005',1);

INSERT INTO TOOLS VALUES ('','Obeng (+)', 'CAT006',1);
INSERT INTO TOOLS VALUES ('','Obeng (-)', 'CAT006',1);
INSERT INTO TOOLS VALUES ('','Obeng Stubby', 'CAT006',1);
INSERT INTO TOOLS VALUES ('','Obeng Electic (+)', 'CAT006',1);
INSERT INTO TOOLS VALUES ('','Obeng Electic (-)', 'CAT006',1);

--INSERT SPAREPART_CATEGORY
INSERT INTO SPAREPART_CATEGORY VALUES ('', 'BEARING');
INSERT INTO SPAREPART_CATEGORY VALUES ('', 'BOLT');
INSERT INTO SPAREPART_CATEGORY VALUES ('', 'LAINNYA');

--INSERT SPAREPART
INSERT INTO SPAREPART VALUES ('', 'BEARING Ball 6203', 'SC001', 50, 2800, 'Bearing Ball 6203');
INSERT INTO SPAREPART VALUES ('', 'BEARING Ball 6303', 'SC001', 50, 30000, 'Bearing Ball 6303');
INSERT INTO SPAREPART VALUES ('', 'BEARING Ball Radial 6301U L', 'SC001', 50, 3200, 'Bearing Ball Radial 6301U L');
INSERT INTO SPAREPART VALUES ('', 'BEARING Radial 6301', 'SC001', 50, 3400, 'Bearing Radial 6301');
INSERT INTO SPAREPART VALUES ('', 'BEARING Radial N/COM', 'SC001', 50, 3600, 'Bearing Radial N/COM');

INSERT INTO SPAREPART VALUES ('', 'BOLT Socket 8x32', 'SC002', 100, 9000, 'BOLT Socket 8x32');
INSERT INTO SPAREPART VALUES ('', 'HEX BOLT M3', 'SC002', 100, 3000, 'HEX BOLT M3');
INSERT INTO SPAREPART VALUES ('', 'HEX BOLT M4', 'SC002', 100, 4000, 'HEX BOLT M4');

INSERT INTO SPAREPART VALUES ('', 'Knalpot Honda Beat', 'SC003', 10, 100000, 'Knalpot Matic');

--INSERT HTRANS
INSERT INTO HTRANS VALUES ('', TO_DATE('19/12/2022', 'DD/MM/YYYY'), 'Fernando', 'Jl. Ngagel Jaya Tengah No. 71', '1234567890123456', '219116828', 'N 1234 RR', 'Honda Vario', 0,'-', 1);
INSERT INTO HTRANS VALUES ('', TO_DATE('20/12/2022', 'DD/MM/YYYY'), 'Jonathan', 'Jl. Ngagel Jaya Tengah No. 71', '1234567890123456', '219116833', 'L 1234 RR', 'Honda PCX', 0,'-', 1);
INSERT INTO HTRANS VALUES ('', TO_DATE('20/12/2022', 'DD/MM/YYYY'), 'Dave', 'Jl. Ngagel Jaya Tengah No. 71', '1234567890123456', '219116824', 'W 1234 RR', 'Honda Beat', 0,'-', 1);

--INSERT DTRANS
INSERT INTO DTRANS VALUES ('', '&LOCALID/'||'HT001', '&LOCALID/'||'S001', 'Bearing Ball 6203', 50000, 1);
INSERT INTO DTRANS VALUES ('', '&LOCALID/'||'HT001', '&LOCALID/'||'S002', 'Bearing Ball 6303', 50000, 1);
INSERT INTO DTRANS VALUES ('', '&LOCALID/'||'HT002', '&LOCALID/'||'S001', 'Bolt Socker 8x32', 100000, 1);
INSERT INTO DTRANS VALUES ('', '&LOCALID/'||'HT002', '&LOCALID/'||'S001', 'Bolt Socker 8x32', 100000, 1);
INSERT INTO DTRANS VALUES ('', '&LOCALID/'||'HT002', '&LOCALID/'||'S001', 'Bearing Ball 6203', 50000, 1);
INSERT INTO DTRANS VALUES ('', '&LOCALID/'||'HT003', '&LOCALID/'||'S009', 'Knalpot Honda Beat', 750000, 1);

commit;
INSERT INTO HTRANS(ID_Transaksi,Tanggal,NAMA_PEMILIK,ALAMAT_PEMILIK,NO_KTP,NPWP,NO_POLISI,DESKRIPSI_KENDARAAN,TOTAL,DARI_CABANG,STATUS) VALUES ('', CURRENT_DATE, 'Dave', 'Jl. Ngagel Jaya Tengah No. 71', '1234567890123456', '219116824', 'W 1234 RR', 'Honda Beat', 0,'-', 1);