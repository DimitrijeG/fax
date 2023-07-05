package manager;

import util.InternalUtil;

import java.util.Collection;
import java.util.LinkedHashMap;

public class ArticleMap {
    public final LinkedHashMap<Integer, String> amenities, roomTypes, addServices;
    public final LinkedHashMap<String, Integer> amenityIds, roomTypeIds, addServiceIds;

    public ArticleMap() {
        amenities = new LinkedHashMap<>();
        roomTypes = new LinkedHashMap<>();
        addServices = new LinkedHashMap<>();
        amenityIds = new LinkedHashMap<>();
        roomTypeIds = new LinkedHashMap<>();
        addServiceIds = new LinkedHashMap<>();
    }

    public ArticleMap(ArticleManager amenityManager, ArticleManager roomTypeManager, ArticleManager addServiceManager) {
        amenities = amenityManager.getTitles();
        roomTypes = roomTypeManager.getTitles();
        addServices = addServiceManager.getTitles();
        amenityIds = InternalUtil.invertMap(amenities);
        roomTypeIds = InternalUtil.invertMap(roomTypes);
        addServiceIds = InternalUtil.invertMap(addServices);
    }

    public String getAmenity(Integer id) {
        return amenities.get(id);
    }

    public Integer getAmenity(String title) {
        return amenityIds.get(title);
    }

    public String getRoomType(Integer id) {
        return roomTypes.get(id);
    }

    public Integer getRoomType(String title) {
        return roomTypeIds.get(title);
    }

    public String getAddService(Integer id) {
        return addServices.get(id);
    }

    public Integer getAddService(String title) {
        return addServiceIds.get(title);
    }

    public Collection<String> getAmenities() {
        return amenities.values();
    }

    public Collection<Integer> getAmenityIds() {
        return amenityIds.values();
    }

    public Collection<String> getRoomTypes() {
        return roomTypes.values();
    }

    public Collection<Integer> getRoomTypeIds() {
        return roomTypeIds.values();
    }

    public Collection<String> getAddServices() {
        return addServices.values();
    }

    public Collection<Integer> getAddServiceIds() {
        return addServiceIds.values();
    }
}
