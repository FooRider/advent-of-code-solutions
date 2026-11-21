#[derive(PartialEq, Eq)]
enum Direction {
    Forward,
    Up,
    Down
}

fn main() {
    //let input = include_str!("../input_sample.txt");
    let input = include_str!("../input.txt");

    let mut x1 = 0;
    let mut y1 = 0;

    let mut x2 = 0;
    let mut y2 = 0;
    let mut aim = 0;
    for (direction, distance) in input.lines().map(parse_line) {
        let (dx1, dy1) = match direction {
            Direction::Forward => (distance, 0),
            Direction::Up => (0, -distance),
            Direction::Down => (0, distance)
        };

        (x1, y1) = (x1 + dx1, y1 + dy1);

        if y1 < 0 { println!("Warning, above water"); }

        let (da, dx2, dy2) = match direction {
            Direction::Down => (distance, 0, 0),
            Direction::Up => (-distance, 0, 0),
            Direction::Forward => (0, distance, aim * distance)
        };

        (aim, x2, y2) = (aim + da, x2 + dx2, y2 + dy2);

        if y2 < 0 { println!("Warning, above water"); }
    }

    println!("Part 1: {x1} {y1} => {}", x1 * y1);
    println!("Part 2: {aim} {x2} {y2} => {}", x2 * y2);
}

fn parse_line(line: &str) -> (Direction, i64) {
    let (dir, n) = line.split_once(' ').unwrap();
    let dir = match dir {
        "forward" => Direction::Forward,
        "down" => Direction::Down,
        "up" => Direction::Up,
        _ => panic!("unknown direction: {dir}"),
    };
    (dir, n.parse().unwrap())
}