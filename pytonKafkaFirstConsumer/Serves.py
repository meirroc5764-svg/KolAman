import geopandas as gpd
from shapely.geometry import Point


class Servers:
    def __init__(self):
        pass

    def Check_Validation(self,data:dict):
        if type(data["lon"]) != float or type(data["lat"] != float):
            return False
        return True
    

    def get_region_with_geopandas(self,file_path: str, lon: float, lat: float) -> str:
        # 1. טעינת קובץ ה-GeoJSON ל-GeoDataFrame
        gdf = gpd.read_file(file_path)

        # 2. יצירת נקודה מתאימה
        pt = Point(lon, lat)

        # 3. סינון השורות שהפוליגון שלהן מכיל את הנקודה
        matched = gdf[gdf.geometry.contains(pt)]

        # 4. החזרת שם האזור אם נמצאה התאמה, אחרת OVERSEAS
        if not matched.empty:
            return matched.iloc[0]["region"]
        return "OVERSEAS"

    