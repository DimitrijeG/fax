package type;

import java.time.LocalDate;
import java.time.ZoneId;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;
import java.util.GregorianCalendar;

import static java.time.temporal.ChronoUnit.DAYS;

public class DateRange implements Comparable<DateRange> {
    private LocalDate start, end;

    public DateRange() {
        start = LocalDate.of(1800, 1, 1);
        end = LocalDate.of(1800, 1, 2);
    }

    public DateRange(LocalDate start, LocalDate end) {
        if (start.equals(end) || start.isAfter(end)) {
            throw new IllegalArgumentException();
        }
        this.start = start;
        this.end = end;
    }

    public boolean contains(DateRange range) {
        return contains(range.start) && contains(range.end);
    }

    public boolean overlaps(DateRange range) {
        return contains(range.start) || contains(range.end);
    }

    public boolean contains(LocalDate date) {
        return (date.equals(start) || date.isAfter(start)) &&
                (date.equals(end) || date.isBefore(end));
    }

    public boolean equals(DateRange range) {
        return range.start.equals(start) && range.end.equals(end);
    }

    public long getDays() {
        return DAYS.between(start, end);
    }

    public LocalDate getStart() {
        return start;
    }

    public void setStart(LocalDate start) {
        this.start = start;
    }

    public LocalDate getEnd() {
        return end;
    }

    public void setEnd(LocalDate end) {
        this.end = end;
    }

    @Override
    public int compareTo(DateRange o) {
        if (start.isBefore(o.getStart())) {
            return -1;
        } else if (start.isAfter(o.getStart())) {
            return 1;
        } else {
            return 0;
        }
    }

    public ArrayList<LocalDate> getDates() {
        ArrayList<LocalDate> dates = new ArrayList<>();

        ZoneId defaultZoneId = ZoneId.systemDefault();
        Date startDate = Date.from(start.atStartOfDay(defaultZoneId).toInstant());
        Date endDate = Date.from(end.atStartOfDay(defaultZoneId).toInstant());

        Calendar calendar = new GregorianCalendar();
        calendar.setTime(startDate);
        while (calendar.getTime().before(endDate)) {
            Date result = calendar.getTime();
            dates.add(result.toInstant().atZone(defaultZoneId).toLocalDate());
            calendar.add(Calendar.DATE, 1);
        }
        return dates;
    }
}
