use std::collections::HashSet;

type Point = (i32, i32);
type Line = (Point, Point);

fn main() {
    //let input = include_str!("../input_sample.txt");
    let input = include_str!("../input.txt");

    let p1: Vec<Line> = input.lines().map(parse_line)
        .filter(|((x1, y1), (x2, y2))| x1 == x2 || y1 == y2)
        .collect();
    let part1_at_least_2 = count_at_least_2(&p1);
    println!("Part 1: {part1_at_least_2}");

    let p2: Vec<Line> = input.lines().map(parse_line).collect();
    let part2_at_least_2 = count_at_least_2(&p2);
    println!("Part 2: {part2_at_least_2}");
}

fn parse_line(line: &str) -> Line {
    let ns: Vec<i32> = line
        .split(" -> ")
        .flat_map(|pair| pair.split(','))
        .map(|s| s.parse().unwrap())
        .collect();

    ((ns[0], ns[1]), (ns[2], ns[3]))
}

fn line_points(((x1, y1), (x2, y2)): Line) -> Vec<Point> {
    let dx = (x2 - x1).signum();
    let dy = (y2 - y1).signum();
    let mut points: Vec<Point> = Vec::new();
    let mut p = (x1, y1);
    points.push(p);
    while p != (x2, y2) {
        p = (p.0 + dx, p.1 + dy);
        points.push(p);
    }
    points
}

fn count_at_least_2(lines: &[Line]) -> usize {
    let mut points_al1: HashSet<Point> = HashSet::new();
    let mut points_al2: HashSet<Point> = HashSet::new();

    for line in lines {
        let points = line_points(*line);
        //println!("{:?}\t{:?}", line, points);

        for p in points {
            if points_al1.contains(&p) {
                points_al2.insert(p);
            } else {
                points_al1.insert(p);
            }
        }
    }

    points_al2.iter().count()
}