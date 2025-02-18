SELECT UPPER(clctn_nm), UPPER(ankr_num) 
FROM ppsowner.anchor 
WHERE UPPER(clctn_nm) IN ('CFR') 
AND UPPER(ankr_num) IN ('CFR-20-404-316-C', 'CFR-20-404-328', 'CFR-20-404-337-C');
