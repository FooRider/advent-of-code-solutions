use std::collections::{HashSet, VecDeque};

fn main() {
    let input = include_str!("../input.txt");
    //let input = include_str!("../input_sample.txt");

    let input : Vec<Vec<u8>> = input.lines()
        .map(|line| line.chars().map(|ch| (ch as u8) - ('0' as u8)).collect()).collect();

    let height = input.len();
    let width = input[0].len();

    let mut cummulative_risk = 0i32;
    let mut low_point_coords: Vec<(usize, usize)> = Vec::new();
    for y in 0..height {
        for x in 0..width {
            if !neighbor_coords(height, width, y, x).iter().any(|(y1, x1)| input[*y1][*x1] <= input[y][x]) {
                low_point_coords.push((y, x));
                cummulative_risk = cummulative_risk + (1 + input[y][x]) as i32;
            }
        }
    }

    println!("Part 1: {cummulative_risk}");

    let mut basin_sizes: Vec<usize> = low_point_coords.iter()
        .map(move |coords| calculate_size_of_basin(&input, coords.0, coords.1))
        .collect();

    basin_sizes.sort_by_key(|size| -(*size as i32));

    let basin_sizes_multiplied = basin_sizes[0] * basin_sizes[1] * basin_sizes[2];
    println!("Part 2: {basin_sizes_multiplied}");
}

fn neighbor_coords(height: usize, width: usize, y: usize, x: usize) -> Vec<(usize, usize)> {
    let result: Vec<(usize, usize)> = [ (-1i32, 0i32), (0i32, -1i32), (1i32, 0i32), (0i32, 1i32) ]
        .iter().map(|(dy, dx)| ((y as i32) + *dy, (x as i32) + *dx))
        .filter(|(y, x)| *y >= 0 && *y < height as i32 && *x >= 0 && *x < width as i32)
        .map(|(y, x)| (y as usize, x as usize))
        .collect();
    result
}

fn calculate_size_of_basin(input: &Vec<Vec<u8>>, low_y: usize, low_x: usize) -> usize {
    let height = input.len();
    let width = input[0].len();

    let mut visited: HashSet<(usize, usize)> = HashSet::new();
    let mut queue: VecDeque<(usize, usize)> = VecDeque::new();

    queue.push_back((low_y, low_x));
    while queue.len() > 0 {
        let (y, x) = queue.pop_front().unwrap();
        if visited.contains(&(y, x)) { continue; }
        if input[y][x] == 9 { continue; }
        visited.insert((y, x));

        for neigh in neighbor_coords(height, width, y, x) {
            queue.push_back(neigh);
        }
    }

    visited.len()
}