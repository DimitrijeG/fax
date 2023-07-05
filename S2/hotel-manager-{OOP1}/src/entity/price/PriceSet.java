package entity.price;


import type.DateRange;

import java.util.ArrayList;
import java.util.TreeSet;

public class PriceSet implements Comparable<PriceSet> {
    private final TreeSet<Price> ranges;

    public PriceSet() {
        ranges = new TreeSet<Price>();
    }

    public PriceSet(TreeSet<Price> ranges) {
        this.ranges = ranges;
    }

    public boolean add(Price price) {
        if (!overlapping(price)) {
            ranges.add(price);
            return true;
        }
        return false;
    }

    public boolean remove(Price price) {
        return false;
    }

    public TreeSet<Price> getPrices() {
        return ranges;
    }

    public boolean overlapping(Price p) {
        DateRange dateRange = p.getDateRange();
        for (Price price : ranges) {
            if (price.getDateRange().overlaps(dateRange)) {
                return true;
            }
        }
        return false;
    }

    @Override
    public int compareTo(PriceSet ps) {
        ArrayList<Price> r1 = new ArrayList<>(ranges);
        ArrayList<Price> r2 = new ArrayList<>(ps.getPrices());
        Integer size1 = r1.size(), size2 = r2.size();
        if (!size1.equals(size2))
            return size1.compareTo(size2);

        int res = 0;
        for (int i = 0; i < size1; i++) {
            res = r1.get(i).compareTo(r2.get(i));
            if (res != 0)
                return res;
        }
        return res;
    }
}
