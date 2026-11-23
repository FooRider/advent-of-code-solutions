use std::collections::{HashSet, VecDeque};

type State = Vec<Vec<u8>>;
type Point = (usize, usize);

fn main() {
    let input = include_str!("../input.txt");
    //let input = include_str!("../input_sample.txt");

    let input : State = input.lines()
        .map(|line| line.chars().map(|ch| (ch as u8) - ('0' as u8)).collect())
        .collect();

    let mut flash_count = 0;
    let mut new_flashes ;
    let mut state = input.clone();
    for _ in 0..100 {
        (state, new_flashes) = step(&state);
        flash_count = flash_count + new_flashes;
    }

    println!("Part 1: {flash_count}");

    let mut new_flashes = 0;
    let mut state = input.clone();
    let mut step_counter = 0;
    while new_flashes < 100 {
        (state, new_flashes) = step(&state);
        step_counter = step_counter + 1;
    }

    println!("Part 2: {step_counter}");
}

fn step(state: &State) -> (State, usize) {
    let mut result = state.clone();

    let h = result.len();
    let w = result[0].len();

    let mut flash_coords: HashSet<Point> = HashSet::new();
    let mut flash_queue: VecDeque<Point> = VecDeque::new();

    for y in 0..h {
        for x in 0..w {
            let new_value = result[y][x] + 1;
            result[y][x] = new_value;
            if new_value >= 10 {
                flash_queue.push_back((y, x));
                flash_coords.insert((y, x));
            }
        }
    }

    while let Some(point) = flash_queue.pop_front() {
        for n in neighbors(&point, h, w) {
            let new_value = result[n.0][n.1] + 1;
            result[n.0][n.1] = new_value;

            if new_value > 9 && !flash_coords.contains(&n) {
                flash_queue.push_back(n);
                flash_coords.insert(n);
            }
        }
    }

    let flash_count = flash_coords.len();
    for (y, x) in flash_coords {
        result[y][x] = 0;
    }

    (result, flash_count)
}

fn neighbors(p: &Point, h: usize, w: usize) -> Vec<Point> {
    [(-1, -1), (0, -1), (1, -1), (1, 0), (1, 1), (0, 1), (-1, 1), (-1, 0)]
        .map(|(dy, dx)| (p.0 as i32 + dy, p.1 as i32 + dx))
        .iter()
        .filter_map(|(y, x)|
            if *y >= 0 && *y < h as i32 && *x >= 0 && *x < w as i32 {
                Some((*y as usize, *x as usize))
            } else {
                None
            }
        )
        .collect()
}