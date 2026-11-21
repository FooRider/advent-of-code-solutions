use std::collections::HashMap;

fn main() {
    //let input = include_str!("../input_sample.txt");
    let input = include_str!("../input.txt");

    let (draws, boards) = input.split_once("\n\n").unwrap();
    let draws: Vec<i32> = draws.split(',').map(|str| str.parse::<i32>().unwrap()).collect();
    let boards = parse_boards(boards);

    let mut scored_boards: Vec<(i32, i32)> = boards.iter().map(|b| solve1_board(&draws, &b)).collect();
    scored_boards.sort_by_key(|(i, _)| *i);

    let (iteration, sum_unmarked) = scored_boards.iter().next().unwrap();
    let last_number = draws[(iteration - 1) as usize];
    let score1 =  sum_unmarked * last_number;
    println!("Part 1:\titeration: {iteration}\tlast_number: {last_number}\tsum_unmarked: {sum_unmarked}\t{}", score1);


    let (iteration, sum_unmarked) = scored_boards.iter().rev().next().unwrap();
    let last_number = draws[(iteration - 1) as usize];
    let score2 =  sum_unmarked * last_number;
    println!("Part 2:\titeration: {iteration}\tlast_number: {last_number}\tsum_unmarked: {sum_unmarked}\t{}", score2);
}

fn solve1_board(draws: &[i32], board: &[Vec<i32>]) -> (i32, i32) {
    let mut iteration = 0;

    let mut marked_masks = [0, 0, 0, 0, 0];
    let mut marked_masks_transposed = [0, 0, 0, 0, 0];

    let board_coords: HashMap<i32, (usize, usize)> =
        board.iter().enumerate()
            .flat_map(|(x, row)| row.iter().enumerate().map(move |(y, &n)| (n, (x, y))))
            .collect();

    for d in draws {
        iteration = iteration + 1;

        match board_coords.get(d) {
            Some((x, y)) => {
                marked_masks[*x] = marked_masks[*x] | (1 << y);
                marked_masks_transposed[*y] = marked_masks_transposed[*y] | (1 << x);
                if [marked_masks, marked_masks_transposed].concat().iter().any(|v| *v == 0x1F) {
                    break;
                }
            },
            None => {}
        }
    }

    let unmarked: Vec<i32> = board.iter().enumerate()
        .flat_map(|(x, row)| {
            let mask = marked_masks[x];
            row.iter().enumerate().filter(move |(y, _)| ((mask >> y) & 1) != 1)
                .map(|(_, v)| *v)
        })
        .collect();

    let unmarked_sum: i32 = unmarked.iter().sum();
    //println!("{:?}\t{}", unmarked, unmarked_sum);

    (iteration, unmarked_sum)
}

fn parse_boards(input: &str) -> Vec<Vec<Vec<i32>>> {
    input
        .split("\n\n")
        .map(|block| {
            block
                .lines()
                .map(|line| {
                    line.split_whitespace()
                        .map(|n| n.parse().unwrap())
                        .collect()
                })
                .collect()
        })
        .collect()
}
